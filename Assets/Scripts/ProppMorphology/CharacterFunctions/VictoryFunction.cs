using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class VictoryFunction : ICharacterFunction
    {
        public VictoryFunction(TaleKind taleKind, string place, Characters characters)
        {
            Place = place;
            ActingCharacter = characters.Hero;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                StorylineString = $"{characters.Hero} побеждает трёхглавого змея";
                AdditionalCharacter1 = characters.Antagonist1;
                AdditionalCharacter2 = characters.Victim;
            }
            else if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                //StorylineString = "Сестра хитростью сажает бабу-ягу в печь";
                AdditionalCharacter1 = characters.Antagonist2;
                AdditionalCharacter2 = characters.Victim;
            }
            else
            {
                StorylineString = $"{characters.Hero} садит дочь бабы-яги в печь, попросив ее показать, как влезать";
                AdditionalCharacter1 = characters.Antagonist2;
            }
        }

        public string Name { get; } = "Победа";

        public string StorylineString { get; }

        public string Place { get; set; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
