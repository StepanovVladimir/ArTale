using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class DepartureFunction : ICharacterFunction
    {
        public DepartureFunction(TaleKind taleKind, string place, Characters characters)
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
        }

        public string Name { get; } = "Отправка";

        public string Place { get; set; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
