using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class TroubleMessageFunction : ICharacterFunction
    {
        public TroubleMessageFunction(TaleKind taleKind, string place, Characters characters)
        {
            Place = place;
            AdditionalCharacter1 = characters.Hero;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                ActingCharacter = characters.Parent;
            }
            else
            {
                ActingCharacter = characters.Antagonist1;
            }
        }

        public string Name { get; } = "Сообщение о беде";

        public string Place { get; set; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
