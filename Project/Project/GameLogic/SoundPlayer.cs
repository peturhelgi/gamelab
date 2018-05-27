using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TheGreatEscape.Menu.MenuManager;

namespace TheGreatEscape.GameLogic
{

    public class SoundPlayer
    {
        public static Song MenuSong, IngameSong, StorySong;
        public SoundEffect GameOver, LevelCompleted;

        public SoundEffect Pickaxe, PickKey, InteractDoor, Dying;

        ContentManager _content;
        private string SoundPath = "Sounds/";
        SoundEffectInstance gameOverInstance, levelCompletedInstance;

        public SoundPlayer() {}

        public SoundPlayer(ContentManager content)
        {
            _content = content;
            MediaPlayer.IsRepeating = true;
        }


        public void LoadSongs()
        {
            MenuSong = _content.Load<Song>(SoundPath + "menu");
            IngameSong = _content.Load<Song>(SoundPath + "ingame");
            StorySong = _content.Load<Song>(SoundPath + "story_song");

            GameOver = _content.Load<SoundEffect>(SoundPath + "game_over");
            LevelCompleted = _content.Load<SoundEffect>(SoundPath + "level_completed");

            Pickaxe = _content.Load<SoundEffect>(SoundPath + "pickaxe");
            PickKey = _content.Load<SoundEffect>(SoundPath + "pick_key");
            InteractDoor = _content.Load<SoundEffect>(SoundPath + "interact_door");
            Dying = _content.Load<SoundEffect>(SoundPath + "dying");

            gameOverInstance = GameOver.CreateInstance();
            levelCompletedInstance = LevelCompleted.CreateInstance();

        }

        public void Play(SoundToPlay sound)
        {
            MediaPlayer.Stop();
            switch (sound)
            {
                case SoundToPlay.Ingame:
                    MediaPlayer.Play(IngameSong);
                    break;
                case SoundToPlay.Menu:
                    MediaPlayer.Play(MenuSong);
                    break;
                case SoundToPlay.Story:
                    MediaPlayer.Play(StorySong);
                    break;
                case SoundToPlay.GameOver:
                    PlaySoundEffect(gameOverInstance);
                    break;
                case SoundToPlay.LevelCompleted:
                    PlaySoundEffect(levelCompletedInstance);
                    break;

            }
        }

        public void PlaySoundEffect(SoundEffectInstance soundEffect)
        {
            if (soundEffect.State == SoundState.Stopped)
            {
                if (soundEffect == levelCompletedInstance)
                    soundEffect.Volume = 0.23f;
                else
                    soundEffect.Volume = 1f;
                soundEffect.Play();
            }
        }

        public void PlayIngameSound(SoundToPlay sound)
        {
            SoundEffect toPlay = null;
            switch (sound)
            {
                case SoundToPlay.Pickaxe:
                    toPlay = Pickaxe;
                    break;
                case SoundToPlay.Key:
                    toPlay = PickKey;
                    break;
                case SoundToPlay.Door:
                    toPlay = InteractDoor;
                    break;
                case SoundToPlay.Dying:
                    toPlay = Dying;
                    break;
                default:
                    toPlay = PickKey;
                    break;
            }

            var Ins = toPlay.CreateInstance();
            Ins.Volume = 1f;
            Ins.Play();

        }

    }

}
