using System.Collections.Generic;
using UnityEngine;

public class PetPartyController //: IPetElementProvider
{
    private List<PetController> _pets = new();

    private PlayerMovement _player;
    private Wagon _wagon;

    public void Init(PlayerMovement player, Wagon wagon, List<PetController> selectedPets)
    {
        _player = player;
        _wagon = wagon;

        RegisterPet(selectedPets);

        SetPetCommand(EPetCommand.PlayerFollow);
    }

    private void RegisterPet(List<PetController> selectedPets)
    {
        _pets.Clear();

        for (int i = 0; i < selectedPets.Count; i++)
        {
            _pets.Add(selectedPets[i]);
        }
    }

    public void UnregisterPet()
    {
        _pets.Clear();
    }

    public void SetPetCommand(EPetCommand commandMode)
    {
        foreach (var pet in _pets)
        {
            pet.SetCommandMode(commandMode);
        }
    }

    public EPetElement GetDominantElement()
    {
        // Fire, Water, Earth, Air 개수 계산
        // 같은 속성 2마리 이상이면 해당 속성
        // 모두 다르면 Magic 같은 별도 결과 반환
        return EPetElement.COUNT;
    }

    // TODO(김익환): 편성된 펫 속성 비중 계산
    //    public EResolvedElement GetCurrentElement()
    //    {
    //        return EResolvedElement.Magic;
    //    }
}

// 이거 고민
//public enum EResolvedElement
//{
//    Fire,
//    Water,
//    Earth,
//    Air,
//    Magic
//}