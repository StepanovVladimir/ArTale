using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class CounteractionDecisionFunction : ICharacterFunction
    {
        public CounteractionDecisionFunction(TaleKind taleKind, string place, Characters characters)
        {
            Place = place;
            ActingCharacter = characters.Hero;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                StorylineString = $"{characters.Hero} соглашается помочь царю";
                AdditionalCharacter1 = characters.Parent;
            }
            else
            {
                StorylineString = "Сестра решается спасти братца";
            }
        }

        public string Name { get; } = "Решение противодействовать";

        public string StorylineString { get; }

        public string Place { get; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
