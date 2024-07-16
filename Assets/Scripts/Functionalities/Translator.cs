using System.Collections.Generic;

public class Translator {

    public readonly Dictionary<string, List<string>> localizationTerms = new Dictionary<string, List<string>>() {
        { "howTheGameWorks",        new List<string>(){ "How the game works", "Как работает игра" } },
        { "generalInfo",            new List<string>(){ "Click the bananas to earn coins. When you earn enough coins you can upgrade your bananas so that you can get more coins from them!", "Щелкайте по бананам, чтобы заработать монеты. Когда Вы заработаете достаточно монет, Вы сможете улучшить свои бананы, чтобы получать с них больше монет!" } },
        { "upgradeInfo",            new List<string>(){ "You can upgrade your bananas from the upgrade menu on the right.", "Вы можете улучшить свои бананы в меню улучшений справа." } },
        { "progressInfo",           new List<string>(){ "You can also check your progress in the game by clicking on your portrait at the top left.", "Вы также можете узнать о своем прогрессе в игре, нажав на свой портрет в левом верхнем углу." } },
        { "totalBananaClicks",      new List<string>(){ "Total bananas clicked", "Всего нажатых бананов" } },
        { "totalBananaUpgrades",    new List<string>(){ "Total banana upgrades", "Общее количество улучшений бананов" } },
        { "currentLanguage",        new List<string>(){ "Current Language", "Выбранный язык" } },
    };
}

public enum Languages { 
    English,
    Russian
}