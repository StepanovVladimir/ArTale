using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ProppMorphology.CharacterFunctions
{
    class TroubleMessageFunction : ICharacterFunction
    {
        public TroubleMessageFunction(TaleKind taleKind, string place, Characters characters, bool swanGeeseAreAntagonist = false)
        {
            Place = place;
            AdditionalCharacter1 = characters.Hero;
            if (taleKind == TaleKind.SnakeKidnapsPrincess)
            {
                StorylineString = "Царь сообщает всем о беде";
                ActingCharacter = characters.Parent;
            }
            else
            {
                StorylineString = "Сестра видит, что братца нету, а вдалеке улетают гуси-лебеди";
                ActingCharacter = characters.Antagonist1;
            }
        }

        public string Name { get; } = "Сообщение о беде";

        public string StorylineString { get; }

        public string Place { get; }

        public string ActingCharacter { get; }

        public string AdditionalCharacter1 { get; }

        public string AdditionalCharacter2 { get; }
    }
}
