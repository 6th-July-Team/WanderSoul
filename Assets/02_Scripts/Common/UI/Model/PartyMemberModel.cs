using UnityEngine;

public class PartyMemberModel : BaseModel
{
    private int _currentHp;
    private int _maxHp;
    private string _name;

    public int CurrentHp 
    {  
        get { return _currentHp; } 
        set 
        {
            if (_currentHp == value)
            {
                return;
            }
            _currentHp = value;
            OnPropertyChanged(nameof(CurrentHp));
        } 
    }

    public int MaxHp
    {
        get { return _maxHp; }
        set
        {
            if (_maxHp == value)
            {
                return;
            }
            _maxHp = value;
            OnPropertyChanged(nameof(MaxHp));
        }
    }

    public string Name
    {
        get { return _name; }
        set
        {
            if (_name == value)
            {
                return;
            }
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public float HpFillAmount
    {
        get
        {
            if (_maxHp == 0)
            {
                return 0f;
            }
            return (float) _currentHp / _maxHp;
        }
    }

    public override void PropertyChangedOnInit()
    {
        OnPropertyChanged(nameof(CurrentHp));
        OnPropertyChanged(nameof(MaxHp));
        OnPropertyChanged(nameof(Name));
    }
}
