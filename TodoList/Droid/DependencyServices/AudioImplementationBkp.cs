/*using System;
using TodoList.Android;
using Android.Media;

[assembly: Xamarin.Forms.Dependency(typeof(AudioImplementation))]

namespace TodoList.Android
{
    public class AudioImplementation : IAudio
    {
        MediaRecorder recorder;
        MediaPlayer player;

        public AudioImplementation() { }

        public void StartRecording(string path)
        {
            try
            {
                if(recorder == null)
                {
                    recorder = new MediaRecorder();
                }

                recorder.SetAudioSource(AudioSource.Mic);
                
                //recorder.SetOutputFormat(OutputFormat.Mpeg4);
                //recorder.SetAudioEncoder(AudioEncoder.Aac);
                //recorder.SetAudioSamplingRate(16000);
                //recorder.SetAudioChannels(2);


                recorder.SetOutputFormat(OutputFormat.Mpeg4);
                recorder.SetAudioEncoder(AudioEncoder.Aac);
                recorder.SetAudioSamplingRate(16000);
                recorder.SetAudioChannels(1);
                recorder.SetAudioEncodingBitRate(16000);

                recorder.SetOutputFile(path);
                recorder.Prepare();
                recorder.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        public void StopRecording()
        {
            try
            {
                if (recorder != null)
                {
                    recorder.Stop();
                    recorder.Reset();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void PlayAudio(string path)
        {
            try
            {
                if (player == null)
                {
                    player = new MediaPlayer();

                    player.Completion += (sender, e) =>
                    {
                        player.Reset();
                    };
                }

                // stop player if it's already playing
                player.Stop();
                player.Reset();

                // play audio
                var file = new Java.IO.FileInputStream(path);
                player.SetDataSource(file.FD);
                player.Prepare();
                player.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void StopAudio()
        {
            try
            {
                if (player != null)
                {
                    player.Stop();
                    player.Reset();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}*/