/*
* 语音合成（Text To Speech，TTS）技术能够自动将任意文字实时转换为连续的
* 自然语音，是一种能够在任何时间、任何地点，向任何人提供语音信息服务的
* 高效便捷手段，非常符合信息时代海量数据、动态更新和个性化查询的需求。
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

/* wav音频头部格式 */
typedef struct _wave_pcm_hdr
{
	char            riff[4];                // = "RIFF"
	int				size_8;                 // = FileSize - 8
	char            wave[4];                // = "WAVE"
	char            fmt[4];                 // = "fmt "
	int				fmt_size;				// = 下一个结构体的大小 : 16

	short int       format_tag;             // = PCM : 1
	short int       channels;               // = 通道数 : 1
	int				samples_per_sec;        // = 采样率 : 8000 | 6000 | 11025 | 16000
	int				avg_bytes_per_sec;      // = 每秒字节数 : samples_per_sec * bits_per_sample / 8
	short int       block_align;            // = 每采样点字节数 : wBitsPerSample / 8
	short int       bits_per_sample;        // = 量化比特数: 8 | 16

	char            data[4];                // = "data";
	int				data_size;              // = 纯数据长度 : FileSize - 44 
} wave_pcm_hdr;

/* 默认wav音频头部数据 */
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


/* 文本合成 */
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
	printf("正在合成 ...\n");
	WriteStream(tmpStream,(byte*)&wav_hdr,sizeof(wav_hdr));
	while (1) 
	{
		/* 获取合成音频 */
		const void* data = QTTSAudioGet(sessionID, &audio_len, &synth_status, &ret);
		if (MSP_SUCCESS != ret)
			break;
		if (NULL != data)
		{
			WriteStream(tmpStream,(byte*)data,audio_len);
		    wav_hdr.data_size += audio_len; //计算data_size大小
		}
		if (MSP_TTS_FLAG_DATA_END == synth_status)
			break;
	}//合成状态synth_status取值请参阅《讯飞语音云API文档》
	printf("\n");
	if (MSP_SUCCESS != ret)
	{
		printf("QTTSAudioGet failed, error code: %d.\n",ret);
		QTTSSessionEnd(sessionID, "AudioGetError");
		return ret;
	}
	/* 修正wav文件头数据的大小 */
	wav_hdr.size_8 = wav_hdr.data_size + (sizeof(wav_hdr) - 8);
	tmpStream->Seek(0,System::IO::SeekOrigin::Begin);
	WriteStream(tmpStream,(byte*)&wav_hdr,sizeof(wav_hdr));
	/* 合成完毕 */
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
	const char* login_params         = "appid = 598816c8, work_dir = .";//登录参数,appid与msc库绑定,请勿随意改动
	/*
	* rdn:           合成音频数字发音方式
	* volume:        合成音频的音量
	* pitch:         合成音频的音调
	* speed:         合成音频对应的语速
	* voice_name:    合成发音人
	* sample_rate:   合成音频采样率
	* text_encoding: 合成文本编码格式
	*
	* 详细参数说明请参阅《iFlytek MSC Reference Manual》
	*/
	const char* session_begin_params = "engine_type = local, voice_name = xiaoyan, text_encoding = GB2312, tts_res_path = fo|res\\tts\\xiaoyan.jet;fo|res\\tts\\common.jet, sample_rate = 8000, speed = 50, volume = 50, pitch = 50, rdn = 2";
	//const char* filename             = "tts_sample.wav"; //合成的语音文件名称
	//const char* text                 = "亲爱的用户，您好，这是一个语音合成示例，感谢您对科大讯飞语音技术的支持！科大讯飞是亚太地区最大的语音上市公司，股票代码：002230"; //合成文本
	/* 用户登录 */
	ret = MSPLogin(NULL, NULL, login_params); //第一个参数是用户名，第二个参数是密码，第三个参数是登录参数，用户名和密码可在http://open.voicecloud.cn注册获取
	if (MSP_SUCCESS != ret)
	{
		printf("MSPLogin failed, error code: %d.\n", ret);
		goto exit ;//登录失败，退出登录
	}

	
	/* 文本合成 */
	printf("开始合成 ...\n");
	MemoryStream^ stream = gcnew MemoryStream();
	ret = text_to_speech(text, filename, session_begin_params,stream);
	if (MSP_SUCCESS != ret)
	{
		printf("text_to_speech failed, error code: %d.\n", ret);
		goto exit;
	}
	YuConvert::PcmToAlaw(stream, filename);
	printf("合成完毕\n");

exit:
	printf("按任意键退出 ...\n");
	_getch();
	MSPLogout(); //退出登录
	return ret;
}


int main()
{
	System::Console::WriteLine("将会生成test.wav文件");
	char * fn = "test.wav";
	TxtToWav("你这是怎么回事呢？表给我扯没用的啦",fn);
	return 0;
}