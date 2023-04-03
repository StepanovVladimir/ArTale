using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class InitialSituationFunction : ICharacterFunction
    {
        public InitialSituationFunction(TaleKind taleKind, string place, Characters characters)
        {
            Place = place;
            ActingCharacter = characters.Parent;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                StorylineString = "Жил-был царь, у него была дочь";
                AdditionalCharacter1 = characters.Victim;
            }
            else if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                StorylineString = "Жили-были старик со старушкой, у них были дочка и сынок маленький";
                AdditionalCharacter1 = characters.Hero;
                AdditionalCharacter2 = characters.Victim;
            }
            else
            {
                StorylineString = $"Жили-были старик со старушкой, у них был сынок {characters.Hero}";
                AdditionalCharacter1 = characters.Hero;
            }
        }

        public string Name { get; } = "Начальная ситуация";

        public string StorylineString { get; }

        public string Place { get; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
