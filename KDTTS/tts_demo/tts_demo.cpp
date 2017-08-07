/*
* �����ϳɣ�Text To Speech��TTS�������ܹ��Զ�����������ʵʱת��Ϊ������
* ��Ȼ��������һ���ܹ����κ�ʱ�䡢�κεص㣬���κ����ṩ������Ϣ�����
* ��Ч����ֶΣ��ǳ�������Ϣʱ���������ݡ���̬���º͸��Ի���ѯ������
*/

#include <stdlib.h>
#include <stdio.h>
#include <windows.h>
#include <conio.h>
#include <errno.h>

#include "../include/qtts.h"
#include "../include/msp_cmn.h"
#include "../include/msp_errors.h"
#include "Convert.h"

#ifdef _WIN64
#pragma comment(lib,"../lib/msc_x64.lib")//x64
#else
#pragma comment(lib,"../lib/msc.lib")//x86
#endif

using namespace System::IO;

/* wav��Ƶͷ����ʽ */
typedef struct _wave_pcm_hdr
{
	char            riff[4];                // = "RIFF"
	int				size_8;                 // = FileSize - 8
	char            wave[4];                // = "WAVE"
	char            fmt[4];                 // = "fmt "
	int				fmt_size;				// = ��һ���ṹ��Ĵ�С : 16

	short int       format_tag;             // = PCM : 1
	short int       channels;               // = ͨ���� : 1
	int				samples_per_sec;        // = ������ : 8000 | 6000 | 11025 | 16000
	int				avg_bytes_per_sec;      // = ÿ���ֽ��� : samples_per_sec * bits_per_sample / 8
	short int       block_align;            // = ÿ�������ֽ��� : wBitsPerSample / 8
	short int       bits_per_sample;        // = ����������: 8 | 16

	char            data[4];                // = "data";
	int				data_size;              // = �����ݳ��� : FileSize - 44 
} wave_pcm_hdr;

/* Ĭ��wav��Ƶͷ������ */
wave_pcm_hdr default_wav_hdr = 
{
	{ 'R', 'I', 'F', 'F' },
	0,
	{'W', 'A', 'V', 'E'},
	{'f', 'm', 't', ' '},
	16,
	1,
	1,
	8000,
	16000,
	2,
	16,
	{'d', 'a', 't', 'a'},
	0  
};

void WriteStream(MemoryStream^ stream, byte * buffer,int len)
{
	for(int i=0;i<len;i++)stream->WriteByte(buffer[i]);
}


/* �ı��ϳ� */
int text_to_speech(const char* src_text, const char* des_path, const char* params,MemoryStream^ tmpStream)
{
	int          ret          = -1;
	//FILE*        fp           = NULL;
	const char*  sessionID    = NULL;
	unsigned int audio_len    = 0;
	wave_pcm_hdr wav_hdr      = default_wav_hdr;
	int          synth_status = MSP_TTS_FLAG_STILL_HAVE_DATA;

	if (NULL == src_text || NULL == des_path)
	{
		printf("params is error!\n");
		return ret;
	}
	
	sessionID = QTTSSessionBegin(params, &ret);
	if (MSP_SUCCESS != ret)
	{
		printf("QTTSSessionBegin failed, error code: %d.\n", ret);
		return ret;
	}
	ret = QTTSTextPut(sessionID, src_text, (unsigned int)strlen(src_text), NULL);
	if (MSP_SUCCESS != ret)
	{
		printf("QTTSTextPut failed, error code: %d.\n",ret);
		QTTSSessionEnd(sessionID, "TextPutError");
		return ret;
	}
	printf("���ںϳ� ...\n");
	WriteStream(tmpStream,(byte*)&wav_hdr,sizeof(wav_hdr));
	while (1) 
	{
		/* ��ȡ�ϳ���Ƶ */
		const void* data = QTTSAudioGet(sessionID, &audio_len, &synth_status, &ret);
		if (MSP_SUCCESS != ret)
			break;
		if (NULL != data)
		{
			WriteStream(tmpStream,(byte*)data,audio_len);
		    wav_hdr.data_size += audio_len; //����data_size��С
		}
		if (MSP_TTS_FLAG_DATA_END == synth_status)
			break;
	}//�ϳ�״̬synth_statusȡֵ����ġ�Ѷ��������API�ĵ���
	printf("\n");
	if (MSP_SUCCESS != ret)
	{
		printf("QTTSAudioGet failed, error code: %d.\n",ret);
		QTTSSessionEnd(sessionID, "AudioGetError");
		return ret;
	}
	/* ����wav�ļ�ͷ���ݵĴ�С */
	wav_hdr.size_8 = wav_hdr.data_size + (sizeof(wav_hdr) - 8);
	tmpStream->Seek(0,System::IO::SeekOrigin::Begin);
	WriteStream(tmpStream,(byte*)&wav_hdr,sizeof(wav_hdr));
	/* �ϳ���� */
	ret = QTTSSessionEnd(sessionID, "Normal");
	if (MSP_SUCCESS != ret)
	{
		printf("QTTSSessionEnd failed, error code: %d.\n",ret);
	}

	return ret;
}


int __stdcall TxtToWav(const char * text,const char * filename)
{
	int         ret                  = MSP_SUCCESS;
	const char* login_params         = "appid = 598816c8, work_dir = .";//��¼����,appid��msc���,��������Ķ�
	/*
	* rdn:           �ϳ���Ƶ���ַ�����ʽ
	* volume:        �ϳ���Ƶ������
	* pitch:         �ϳ���Ƶ������
	* speed:         �ϳ���Ƶ��Ӧ������
	* voice_name:    �ϳɷ�����
	* sample_rate:   �ϳ���Ƶ������
	* text_encoding: �ϳ��ı������ʽ
	*
	* ��ϸ����˵������ġ�iFlytek MSC Reference Manual��
	*/
	const char* session_begin_params = "engine_type = local, voice_name = xiaoyan, text_encoding = GB2312, tts_res_path = fo|res\\tts\\xiaoyan.jet;fo|res\\tts\\common.jet, sample_rate = 8000, speed = 50, volume = 50, pitch = 50, rdn = 2";
	//const char* filename             = "tts_sample.wav"; //�ϳɵ������ļ�����
	//const char* text                 = "�װ����û������ã�����һ�������ϳ�ʾ������л���Կƴ�Ѷ������������֧�֣��ƴ�Ѷ������̫���������������й�˾����Ʊ���룺002230"; //�ϳ��ı�
	/* �û���¼ */
	ret = MSPLogin(NULL, NULL, login_params); //��һ���������û������ڶ������������룬�����������ǵ�¼�������û������������http://open.voicecloud.cnע���ȡ
	if (MSP_SUCCESS != ret)
	{
		printf("MSPLogin failed, error code: %d.\n", ret);
		goto exit ;//��¼ʧ�ܣ��˳���¼
	}

	
	/* �ı��ϳ� */
	printf("��ʼ�ϳ� ...\n");
	MemoryStream^ stream = gcnew MemoryStream();
	ret = text_to_speech(text, filename, session_begin_params,stream);
	if (MSP_SUCCESS != ret)
	{
		printf("text_to_speech failed, error code: %d.\n", ret);
		goto exit;
	}
	YuConvert::PcmToAlaw(stream, filename);
	printf("�ϳ����\n");

exit:
	printf("��������˳� ...\n");
	_getch();
	MSPLogout(); //�˳���¼
	return ret;
}


int main()
{
	System::Console::WriteLine("��������test.wav�ļ�");
	char * fn = "test.wav";
	TxtToWav("��������ô�����أ�����ҳ�û�õ���",fn);
	return 0;
}