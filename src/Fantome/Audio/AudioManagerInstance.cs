using Fantome.Utilities;

namespace Fantome.Audio;

[GlobalClass, StaticAutoloadSingleton("Fantome.Audio", "AudioManager")]
public partial class AudioManagerInstance : Node
{
    [Export] public AudioStreamPlayer Music;

    public override void _Ready()
    {
        base._Ready();

        Music = new AudioStreamPlayer();
        AddChild(Music);
    }

    public void PlayMusic(AudioStream stream)
    {
        Music.Stream = stream;
        Music.Play();
    }

    public void PlaySoundEffect(AudioStream stream)
    {
        AudioStreamPlayer sfx = new AudioStreamPlayer();
        sfx.Stream = stream;
        sfx.Finished += () => sfx.QueueFree();
        
        AddChild(sfx);
        sfx.Play();
    }
}