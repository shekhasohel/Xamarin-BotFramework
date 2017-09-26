using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList
{
    public interface IAudio
    {
        void StartRecording(string path);

        void StopRecording();

        void PlayAudio(string path);

        void StopAudio();
    }
}
