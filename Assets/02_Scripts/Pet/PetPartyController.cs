using System.Collections.Generic;

public class PetPartyController
{
    private PetCommandController _petCommandController;

    private List<PetController> _pets = new();

    // TODO(김익환): 가지고 있는 펫 전부에게 명령 때리기
    // TODO(김익환): 펫 속성 비중 확인

    public void RegisterPet(PetController pet)
    {
        if(!_pets.Contains(pet))
        {
            _pets.Add(pet);
        }
    }

    public void UnregisterPet(PetController pet)
    {
        if(_pets.Contains(pet))
        {
            _pets.Remove(pet);
        }
    }

    public void SetPetCommand(EPetCommand commandMode)
    {
        _petCommandController.SetCommandMode(commandMode);
        foreach(var pet in _pets)
        {
            //pet.SetCommandState(_petCommandController.CurrentCommandState);
        } 
    }

    public EPetElement GetDominantElement()
    {
        // Fire, Water, Earth, Air 개수 계산
        // 같은 속성 2마리 이상이면 해당 속성
        // 모두 다르면 Magic 같은 별도 결과 반환
        return EPetElement.COUNT;
    }

    //private void Update()
    //{
    //    _timer += Time.deltaTime;

    //    if (_timer < _decisionInterval)
    //        return;

    //    _timer = 0f;

    //    EvaluatePets();
    //}

    //private void EvaluatePets()
    //{
    //    foreach (var pet in _pets)
    //    {
    //        if (pet == null || !pet.IsAlive)
    //            continue;

    //        var targets = pet.ScanTargets();

    //        var context = new PetCommandContext(
    //            pet: pet,
    //            player: _player,
    //            cart: _cart,
    //            detectedTargets: targets,
    //            attackRange: 2f,
    //            followDistance: 2f,
    //            maxGuardDistance: 8f
    //        );

    //        PetCommandResult result = _commandController.Evaluate(context);
    //        pet.ApplyCommandResult(result);
    //    }
    //}
}

// 이거 고민
public enum EResolvedElement
{
    Fire,
    Water,
    Earth,
    Air,
    Magic
}