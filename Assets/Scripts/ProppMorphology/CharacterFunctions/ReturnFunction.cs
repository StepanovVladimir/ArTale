using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class ReturnFunction : ICharacterFunction
    {
        public ReturnFunction(TaleKind taleKind, string place, Characters characters)
        {
            SceneIndex = 11;
            Place = place;
            ActingCharacter = characters.Hero;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                StorylineString = $"{characters.Hero} и царевна возвращаются к царю во дворец";
                AdditionalCharacter1 = characters.Victim;
                AdditionalCharacter2 = characters.Parent;
            }
            else if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                StorylineString = "Сестра и братец возвращаются домой к отцу и матери";
                AdditionalCharacter1 = characters.Victim;
                AdditionalCharacter2 = characters.Parent;
            }
            else
            {
                StorylineString = $"{characters.Hero} возвращается домой к отцу и матери";
                AdditionalCharacter1 = characters.Parent;
            }
        }

        public string Name { get; } = "Возвращение";

        public string StorylineString { get; }

        public int SceneIndex { get; }

        public string Place { get; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
