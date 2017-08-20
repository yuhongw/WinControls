#pragma managed
using namespace System;
using namespace System::IO;
using namespace NAudio::Wave;

ref class YuConvert
{
public:
	static void PcmToAlaw(MemoryStream^ stream, const char * fout)
	{
		//String^ fn = gcnew String(fin) ;
		stream->Seek(0,SeekOrigin::Begin);
		String^ fnout = gcnew String(fout);
		WaveFileReader^ reader = gcnew WaveFileReader(stream);
		WaveFormat^ waveFormat = WaveFormat::CreateALawFormat(8000, 1);
		WaveFormatConversionStream^ convertionStream = gcnew WaveFormatConversionStream(waveFormat, reader);
		WaveFileWriter::CreateWaveFile(fnout, convertionStream);
		convertionStream->Close();
		reader->Close();
	}
};