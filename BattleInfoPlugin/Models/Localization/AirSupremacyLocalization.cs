using BattleInfoPlugin.Properties;

namespace BattleInfoPlugin.Models.Localization
{
    class AirSupremacyLocalization
    {
        public static string GetResource(AirSupremacy airSupremacyResult)
        {
            switch (airSupremacyResult)
            {
                case AirSupremacy.航空戦なし:
                    return Resources.Air_Supremacy_None;
                case AirSupremacy.航空均衡:
                    return Resources.Air_Supremacy_Parity;
                case AirSupremacy.制空権確保:
                    return Resources.Air_Supremacy_Supremacy;
                case AirSupremacy.航空優勢:
                    return Resources.Air_Supremacy_Superiority;
                case AirSupremacy.航空劣勢:
                    return Resources.Air_Supremacy_Denial;
                case AirSupremacy.制空権損失:
                    return Resources.Air_Supremacy_Incapability;
            }
            return "";
        }
    }
}
