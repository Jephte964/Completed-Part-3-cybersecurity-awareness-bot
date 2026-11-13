using System;
using System.IO;
using System.Media;
using System.Windows;

namespace CybersecurityAwarenessBot.Classes
{
    public class AudioPlayer
    {
        private SoundPlayer player;

        public void PlayGreeting()
        {
            try
            {
                string audioPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Audio",
                    "greeting.wav"
                );

                if (!File.Exists(audioPath))
                {
                    MessageBox.Show(
                        "Audio file not found!\n\n" +
                        "Expected location:\n" + audioPath +
                        "\n\nPlease check that greeting.wav is inside the Audio folder and that Copy to Output Directory is set to Copy if newer.",
                        "Audio File Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );

                    return;
                }

                player = new SoundPlayer(audioPath);
                player.Load();
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to play greeting audio:\n" + ex.Message,
                    "Audio Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}