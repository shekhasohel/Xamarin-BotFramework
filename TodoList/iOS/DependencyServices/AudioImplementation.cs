using System;
using TodoList.iOS;
using AVFoundation;
using Foundation;
using TodoList;

[assembly: Xamarin.Forms.Dependency(typeof(AudioImplementation))]

namespace TodoList.iOS
{
    public class AudioImplementation : IAudio
    {
        AVAudioRecorder recorder;
        NSError error;
        NSUrl url;
        NSDictionary settings;

        AVAudioPlayer player;

        public AudioImplementation()
        {
            // Initialize
            ActivateAudioSession();
        }

        public void StartRecording(string path)
        {
            try
            {
                path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), path); // this line is added to place file in iOS temp directory, may not be required in Halza app
                var audioSession = AVAudioSession.SharedInstance();
                audioSession.RequestRecordPermission(delegate (bool granted)
                {
                    if (granted)
                    {


                        var err = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);
                        if (err != null)
                        {
                            Console.WriteLine("audioSession: {0}", err);
                        }
                        err = audioSession.SetActive(true);
                        if (err != null)
                        {
                            Console.WriteLine("audioSession: {0}", err);
                        }

                        url = NSUrl.FromFilename(path);
                        //set up the NSObject Array of values that will be combined with the keys to make the NSDictionary
                        NSObject[] values = new NSObject[]
                        {
                    NSNumber.FromFloat (16000), //Sample Rate
                    NSNumber.FromInt32 ((int)AudioToolbox.AudioFormatType.MPEG4AAC), //AVFormat
                    //NSNumber.FromInt32 ((int)AudioToolbox.AudioFormatType.LinearPCM), //AVFormat
                    NSNumber.FromInt32 (2), //Channels
                    NSNumber.FromInt32 (16), //PCMBitDepth
                    NSNumber.FromBoolean (false), //IsBigEndianKey
                    NSNumber.FromBoolean (false) //IsFloatKey
                        };

                        //Set up the NSObject Array of keys that will be combined with the values to make the NSDictionary
                        NSObject[] keys = new NSObject[]
                        {
                    AVAudioSettings.AVSampleRateKey,
                    AVAudioSettings.AVFormatIDKey,
                    AVAudioSettings.AVNumberOfChannelsKey,
                    AVAudioSettings.AVLinearPCMBitDepthKey,
                    AVAudioSettings.AVLinearPCMIsBigEndianKey,
                    AVAudioSettings.AVLinearPCMIsFloatKey
                        };

                        //Set Settings with the Values and Keys to create the NSDictionary
                        settings = NSDictionary.FromObjectsAndKeys(values, keys);

                        //Set recorder parameters
                        recorder = AVAudioRecorder.Create(url, new AudioSettings(settings), out error);

                        //Set Recorder to Prepare To Record
                        recorder.PrepareToRecord();

                        recorder.Record();
                    }
                });
            }
            catch (Exception ex)
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
                    recorder.Dispose();
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
                path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), path); // this line is added to place file in iOS temp directory, may not be required in Halza app
                if (player != null)
                {
                    //Stop and dispose of any background music
                    player.Stop();
                    player.Dispose();
                }

                // Initialize background music
                NSUrl songURL = new NSUrl("Sounds/" + path);
                NSError err;
                player = new AVAudioPlayer(songURL, "acc", out err);
                player.FinishedPlaying += delegate
                {
                    player = null;
                };
                //player.NumberOfLoops = 0;
                player.Play();
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
                    player.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ActivateAudioSession()
        {
            // Initialize Audio
            var session = AVAudioSession.SharedInstance();
            session.SetCategory(AVAudioSessionCategory.Ambient);
            session.SetActive(true);
        }

        public void DeactivateAudioSession()
        {
            var session = AVAudioSession.SharedInstance();
            session.SetActive(false);
        }

        public void ReactivateAudioSession()
        {
            var session = AVAudioSession.SharedInstance();
            session.SetActive(true);
        }
    }
}