using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models;
using BattleInfoPlugin.Models.Raw;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Json;

namespace BattleInfoPluginTest.Models
{
    [TestClass()]
    public class BattleDataTests
    {
        [TestMethod()]
        public void UpdateTest()
        {
            var json = "{  \"api_deck_id\": 1,  \"api_formation\": [    11,    4,    1  ],  \"api_f_nowhps\": [    47,    46  ],  \"api_f_maxhps\": [    47,    46  ],  \"api_f_nowhps_combined\": [    52,    31,    31  ],  \"api_f_maxhps_combined\": [    52,    31,    31  ],  \"api_fParam\": [    [      64,      70,      128,      61    ],    [      75,      60,      76,      66    ]  ],  \"api_fParam_combined\": [    [      74,      68,      78,      68    ],    [      52,      90,      70,      50    ],    [      53,      90,      72,      50    ]  ],  \"api_ship_ke\": [    1572,    1572,    1571,    1776,    1776  ],  \"api_ship_lv\": [    50,    50,    50,    1,    1  ],  \"api_e_nowhps\": [    48,    48,    45,    \"N/A\",    \"N/A\"  ],  \"api_e_maxhps\": [    48,    48,    45,    \"N/A\",    \"N/A\"  ],  \"api_eSlot\": [    [      1515,      1515,      1515,      -1,      -1    ],    [      1515,      1515,      1515,      -1,      -1    ],    [      1515,      1515,      1503,      -1,      -1    ],    [      1547,      1574,      1574,      -1,      -1    ],    [      1547,      1574,      1574,      -1,      -1    ]  ],  \"api_eParam\": [    [      30,      135,      0,      42    ],    [      30,      135,      0,      42    ],    [      30,      100,      0,      30    ],    [      15,      0,      15,      35    ],    [      15,      0,      15,      35    ]  ],  \"api_smoke_type\": 0,  \"api_balloon_cell\": 0,  \"api_atoll_cell\": 0,  \"api_midnight_flag\": 0,  \"api_search\": [    1,    1  ],  \"api_stage_flag\": [    1,    1,    1  ],  \"api_kouku\": {    \"api_plane_from\": [      null,      [        4,        5      ]    ],    \"api_stage1\": {      \"api_f_count\": 0,      \"api_f_lostcount\": 0,      \"api_e_count\": 144,      \"api_e_lostcount\": 2,      \"api_disp_seiku\": 4,      \"api_touch_plane\": [        -1,        -1      ]    },    \"api_stage2\": {      \"api_f_count\": 0,      \"api_f_lostcount\": 0,      \"api_e_count\": 90,      \"api_e_lostcount\": 75,      \"api_air_fire\": {        \"api_idx\": 0,        \"api_kind\": 39,        \"api_use_items\": [          363,          362        ]      }    },    \"api_stage3\": {      \"api_frai_flag\": [        0,        1      ],      \"api_erai_flag\": [        0,        0,        0,        0,        0      ],      \"api_fbak_flag\": [        0,        0      ],      \"api_ebak_flag\": [        0,        0,        0,        0,        0      ],      \"api_fcl_flag\": [        0,        0      ],      \"api_ecl_flag\": [        0,        0,        0,        0,        0      ],      \"api_fdam\": [        0,        0      ],      \"api_edam\": [        0,        0,        0,        0,        0      ],      \"api_f_sp_list\": [        null,        null      ],      \"api_e_sp_list\": [        null,        null,        null,        null,        null      ]    },    \"api_stage3_combined\": {      \"api_frai_flag\": [        0,        0,        1      ],      \"api_fbak_flag\": [        0,        0,        0      ],      \"api_fcl_flag\": [        0,        0,        0      ],      \"api_fdam\": [        0,        0,        0      ],      \"api_f_sp_list\": [        null,        null,        null      ]    }  },  \"api_support_flag\": 0,  \"api_support_info\": null,  \"api_opening_taisen_flag\": 1,  \"api_opening_taisen\": {    \"api_at_eflag\": [      0,      0,      0    ],    \"api_at_list\": [      6,      8,      7    ],    \"api_at_type\": [      0,      0,      0    ],    \"api_df_list\": [      [        2      ],      [        0      ],      [        1      ]    ],    \"api_si_list\": [      [        \"288\"      ],      [        \"439\"      ],      [        \"439\"      ]    ],    \"api_cl_list\": [      [        2      ],      [        1      ],      [        1      ]    ],    \"api_damage\": [      [        266.1      ],      [        154      ],      [        153      ]    ]  },  \"api_opening_flag\": 0,  \"api_opening_atack\": null,  \"api_hourai_flag\": [    1,    0,    1,    0  ],  \"api_hougeki1\": {    \"api_at_eflag\": [      1,      1    ],    \"api_at_list\": [      4,      3    ],    \"api_at_type\": [      0,      0    ],    \"api_df_list\": [      [        1      ],      [        0      ]    ],    \"api_si_list\": [      [        -1      ],      [        -1      ]    ],    \"api_cl_list\": [      [        1      ],      [        0      ]    ],    \"api_damage\": [      [        16      ],      [        0      ]    ]  },  \"api_hougeki3\": {    \"api_at_eflag\": [      1,      1    ],    \"api_at_list\": [      4,      3    ],    \"api_at_type\": [      0,      0    ],    \"api_df_list\": [      [        6      ],      [        8      ]    ],    \"api_si_list\": [      [        -1      ],      [        -1      ]    ],    \"api_cl_list\": [      [        0      ],      [        0      ]    ],    \"api_damage\": [      [        0      ],      [        0      ]    ]  },  \"api_raigeki\": null}";
            var serializer = new DataContractJsonSerializer(typeof(combined_battle_battle_water));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var input = (combined_battle_battle_water)serializer.ReadObject(ms);

            BattleData battleData = BattleData.Current;
            battleData.Update(input);

            //Assert.Fail();
            Console.WriteLine("111");
        }
    }
}
