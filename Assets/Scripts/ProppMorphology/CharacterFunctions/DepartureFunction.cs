using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class DepartureFunction : ICharacterFunction
    {
        public DepartureFunction(TaleKind taleKind, string place, Characters characters, bool itWasTravelGuide)
        {
            Place = place;
            if (taleKind != TaleKind.BoyEscapesFromBabaYaga)
            {
                ActingCharacter = characters.Hero;
            }
            else
            {
                ActingCharacter = characters.Antagonist1;
                AdditionalCharacter1 = characters.Hero;
            }

            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                if (!itWasTravelGuide)
                {
                    StorylineString = $"{characters.Hero} отправляется в логово трёхглавого змея";
                }
                else
                {
                    StorylineString = $"{characters.Hero} отправляется в лес";
                }
            }
            else if (taleKind == TaleKind.SisterSavesBrotherFromBabaYaga)
            {
                if (!itWasTravelGuide)
                {
                    StorylineString = "Сестра отправляется в лес к избушке на курьих ножках";
                }
                else
                {
                    StorylineString = "Сестра отправляется в лес";
                }
            }
            else
            {
                if (characters.Hero.Equals("Ивашко"))
                {
                    StorylineString = "Баба-яга отправилась с Ивашкой в избушку на курьих ножках";
                }
                else
                {
                    StorylineString = "Баба-яга отправилась с Терешечкой в избушку на курьих ножках";
                }
            }
        }

        public string Name { get; } = "Отправка";

        public string StorylineString { get; }

        public string Place { get; set; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
