using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Scriptable Objects/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    [SerializeField]
    private EventReference playerFootstep;
    public string PlayerFootstep => playerFootstep.Guid.ToString();

    [SerializeField]
    private EventReference gunShot;
    public string GunShot => gunShot.Guid.ToString();

    [SerializeField]
    private EventReference blood;
    public string Blood => blood.Guid.ToString();

    [Header("Monster")]
    [SerializeField]
    private EventReference monster_attack;
    public string Monster_attack => monster_attack.Guid.ToString();
    [SerializeField]
    private EventReference monster_damage;
    public string Monster_damage => monster_damage.Guid.ToString();
    [SerializeField]
    private EventReference monster_death;
    public string Monster_death => monster_death.Guid.ToString();
    [SerializeField]
    private EventReference monster_footstep;
    public string Monster_footstep => monster_footstep.Guid.ToString();

    [Header("Music")]
    [SerializeField]
    private EventReference main_theme;
    public string Main_theme => main_theme.Guid.ToString();
}
