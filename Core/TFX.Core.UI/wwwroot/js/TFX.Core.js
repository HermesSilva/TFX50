"use strict";
var XKey;
(function (XKey) {
    XKey[XKey["K_CANCEL"] = 3] = "K_CANCEL";
    XKey[XKey["K_HELP"] = 6] = "K_HELP";
    XKey[XKey["K_BACK_SPACE"] = 8] = "K_BACK_SPACE";
    XKey[XKey["K_TAB"] = 9] = "K_TAB";
    XKey[XKey["K_CLEAR"] = 12] = "K_CLEAR";
    XKey[XKey["K_RETURN"] = 13] = "K_RETURN";
    XKey[XKey["K_ENTER"] = 14] = "K_ENTER";
    XKey[XKey["K_SHIFT"] = 16] = "K_SHIFT";
    XKey[XKey["K_CONTROL"] = 17] = "K_CONTROL";
    XKey[XKey["K_ALT"] = 18] = "K_ALT";
    XKey[XKey["K_PAUSE"] = 19] = "K_PAUSE";
    XKey[XKey["K_CAPS_LOCK"] = 20] = "K_CAPS_LOCK";
    XKey[XKey["K_ESCAPE"] = 27] = "K_ESCAPE";
    XKey[XKey["K_SPACE"] = 32] = "K_SPACE";
    XKey[XKey["K_PAGE_UP"] = 33] = "K_PAGE_UP";
    XKey[XKey["K_PAGE_DOWN"] = 34] = "K_PAGE_DOWN";
    XKey[XKey["K_END"] = 35] = "K_END";
    XKey[XKey["K_HOME"] = 36] = "K_HOME";
    XKey[XKey["K_LEFT"] = 37] = "K_LEFT";
    XKey[XKey["K_UP"] = 38] = "K_UP";
    XKey[XKey["K_RIGHT"] = 39] = "K_RIGHT";
    XKey[XKey["K_DOWN"] = 40] = "K_DOWN";
    XKey[XKey["K_PRINTSCREEN"] = 44] = "K_PRINTSCREEN";
    XKey[XKey["K_INSERT"] = 45] = "K_INSERT";
    XKey[XKey["K_DELETE"] = 46] = "K_DELETE";
    XKey[XKey["K_0"] = 48] = "K_0";
    XKey[XKey["K_1"] = 49] = "K_1";
    XKey[XKey["K_2"] = 50] = "K_2";
    XKey[XKey["K_3"] = 51] = "K_3";
    XKey[XKey["K_4"] = 52] = "K_4";
    XKey[XKey["K_5"] = 53] = "K_5";
    XKey[XKey["K_6"] = 54] = "K_6";
    XKey[XKey["K_7"] = 55] = "K_7";
    XKey[XKey["K_8"] = 56] = "K_8";
    XKey[XKey["K_9"] = 57] = "K_9";
    XKey[XKey["K_SEMICOLON"] = 59] = "K_SEMICOLON";
    XKey[XKey["K_EQUALS"] = 61] = "K_EQUALS";
    XKey[XKey["K_A"] = 65] = "K_A";
    XKey[XKey["K_B"] = 66] = "K_B";
    XKey[XKey["K_C"] = 67] = "K_C";
    XKey[XKey["K_D"] = 68] = "K_D";
    XKey[XKey["K_E"] = 69] = "K_E";
    XKey[XKey["K_F"] = 70] = "K_F";
    XKey[XKey["K_G"] = 71] = "K_G";
    XKey[XKey["K_H"] = 72] = "K_H";
    XKey[XKey["K_I"] = 73] = "K_I";
    XKey[XKey["K_J"] = 74] = "K_J";
    XKey[XKey["K_K"] = 75] = "K_K";
    XKey[XKey["K_L"] = 76] = "K_L";
    XKey[XKey["K_M"] = 77] = "K_M";
    XKey[XKey["K_N"] = 78] = "K_N";
    XKey[XKey["K_O"] = 79] = "K_O";
    XKey[XKey["K_P"] = 80] = "K_P";
    XKey[XKey["K_Q"] = 81] = "K_Q";
    XKey[XKey["K_R"] = 82] = "K_R";
    XKey[XKey["K_S"] = 83] = "K_S";
    XKey[XKey["K_T"] = 84] = "K_T";
    XKey[XKey["K_U"] = 85] = "K_U";
    XKey[XKey["K_V"] = 86] = "K_V";
    XKey[XKey["K_W"] = 87] = "K_W";
    XKey[XKey["K_X"] = 88] = "K_X";
    XKey[XKey["K_Y"] = 89] = "K_Y";
    XKey[XKey["K_Z"] = 90] = "K_Z";
    XKey[XKey["K_CONTEXT_MENU"] = 93] = "K_CONTEXT_MENU";
    XKey[XKey["K_NUMPAD0"] = 96] = "K_NUMPAD0";
    XKey[XKey["K_NUMPAD1"] = 97] = "K_NUMPAD1";
    XKey[XKey["K_NUMPAD2"] = 98] = "K_NUMPAD2";
    XKey[XKey["K_NUMPAD3"] = 99] = "K_NUMPAD3";
    XKey[XKey["K_NUMPAD4"] = 100] = "K_NUMPAD4";
    XKey[XKey["K_NUMPAD5"] = 101] = "K_NUMPAD5";
    XKey[XKey["K_NUMPAD6"] = 102] = "K_NUMPAD6";
    XKey[XKey["K_NUMPAD7"] = 103] = "K_NUMPAD7";
    XKey[XKey["K_NUMPAD8"] = 104] = "K_NUMPAD8";
    XKey[XKey["K_NUMPAD9"] = 105] = "K_NUMPAD9";
    XKey[XKey["K_MULTIPLY"] = 106] = "K_MULTIPLY";
    XKey[XKey["K_ADD"] = 107] = "K_ADD";
    XKey[XKey["K_SEPARATOR"] = 108] = "K_SEPARATOR";
    XKey[XKey["K_SUBTRACT"] = 109] = "K_SUBTRACT";
    XKey[XKey["K_DECIMAL"] = 110] = "K_DECIMAL";
    XKey[XKey["K_DIVIDE"] = 111] = "K_DIVIDE";
    XKey[XKey["K_F1"] = 112] = "K_F1";
    XKey[XKey["K_F2"] = 113] = "K_F2";
    XKey[XKey["K_F3"] = 114] = "K_F3";
    XKey[XKey["K_F4"] = 115] = "K_F4";
    XKey[XKey["K_F5"] = 116] = "K_F5";
    XKey[XKey["K_F6"] = 117] = "K_F6";
    XKey[XKey["K_F7"] = 118] = "K_F7";
    XKey[XKey["K_F8"] = 119] = "K_F8";
    XKey[XKey["K_F9"] = 120] = "K_F9";
    XKey[XKey["K_F10"] = 121] = "K_F10";
    XKey[XKey["K_F11"] = 122] = "K_F11";
    XKey[XKey["K_F12"] = 123] = "K_F12";
    XKey[XKey["K_F13"] = 124] = "K_F13";
    XKey[XKey["K_F14"] = 125] = "K_F14";
    XKey[XKey["K_F15"] = 126] = "K_F15";
    XKey[XKey["K_F16"] = 127] = "K_F16";
    XKey[XKey["K_F17"] = 128] = "K_F17";
    XKey[XKey["K_F18"] = 129] = "K_F18";
    XKey[XKey["K_F19"] = 130] = "K_F19";
    XKey[XKey["K_F20"] = 131] = "K_F20";
    XKey[XKey["K_F21"] = 132] = "K_F21";
    XKey[XKey["K_F22"] = 133] = "K_F22";
    XKey[XKey["K_F23"] = 134] = "K_F23";
    XKey[XKey["K_F24"] = 135] = "K_F24";
    XKey[XKey["K_NUM_LOCK"] = 144] = "K_NUM_LOCK";
    XKey[XKey["K_SCROLL_LOCK"] = 145] = "K_SCROLL_LOCK";
    XKey[XKey["K_COMMA"] = 188] = "K_COMMA";
    XKey[XKey["K_PERIOD"] = 190] = "K_PERIOD";
    XKey[XKey["K_SLASH"] = 191] = "K_SLASH";
    XKey[XKey["K_BACK_QUOTE"] = 192] = "K_BACK_QUOTE";
    XKey[XKey["K_OPEN_BRACKET"] = 219] = "K_OPEN_BRACKET";
    XKey[XKey["K_BACK_SLASH"] = 220] = "K_BACK_SLASH";
    XKey[XKey["K_CLOSE_BRACKET"] = 221] = "K_CLOSE_BRACKET";
    XKey[XKey["K_QUOTE"] = 222] = "K_QUOTE";
    XKey[XKey["K_META"] = 224] = "K_META";
})(XKey || (XKey = {}));
var XMouseButton;
(function (XMouseButton) {
    XMouseButton[XMouseButton["None"] = 0] = "None";
    XMouseButton[XMouseButton["Left"] = 1] = "Left";
    XMouseButton[XMouseButton["Right"] = 2] = "Right";
})(XMouseButton || (XMouseButton = {}));
var Maps = [
    {
        'base': 'A',
        'letters': /[\u0041\u24B6\uFF21\u00C0\u00C1\u00C2\u1EA6\u1EA4\u1EAA\u1EA8\u00C3\u0100\u0102\u1EB0\u1EAE\u1EB4\u1EB2\u0226\u01E0\u00C4\u01DE\u1EA2\u00C5\u01FA\u01CD\u0200\u0202\u1EA0\u1EAC\u1EB6\u1E00\u0104\u023A\u2C6F]/g
    },
    { 'base': 'AA', 'letters': /[\uA732]/g },
    { 'base': 'AE', 'letters': /[\u00C6\u01FC\u01E2]/g },
    { 'base': 'AO', 'letters': /[\uA734]/g },
    { 'base': 'AU', 'letters': /[\uA736]/g },
    { 'base': 'AV', 'letters': /[\uA738\uA73A]/g },
    { 'base': 'AY', 'letters': /[\uA73C]/g },
    { 'base': 'B', 'letters': /[\u0042\u24B7\uFF22\u1E02\u1E04\u1E06\u0243\u0182\u0181]/g },
    { 'base': 'C', 'letters': /[\u0043\u24B8\uFF23\u0106\u0108\u010A\u010C\u00C7\u1E08\u0187\u023B\uA73E]/g },
    { 'base': 'D', 'letters': /[\u0044\u24B9\uFF24\u1E0A\u010E\u1E0C\u1E10\u1E12\u1E0E\u0110\u018B\u018A\u0189\uA779]/g },
    { 'base': 'DZ', 'letters': /[\u01F1\u01C4]/g },
    { 'base': 'Dz', 'letters': /[\u01F2\u01C5]/g },
    {
        'base': 'E',
        'letters': /[\u0045\u24BA\uFF25\u00C8\u00C9\u00CA\u1EC0\u1EBE\u1EC4\u1EC2\u1EBC\u0112\u1E14\u1E16\u0114\u0116\u00CB\u1EBA\u011A\u0204\u0206\u1EB8\u1EC6\u0228\u1E1C\u0118\u1E18\u1E1A\u0190\u018E]/g
    },
    { 'base': 'F', 'letters': /[\u0046\u24BB\uFF26\u1E1E\u0191\uA77B]/g },
    {
        'base': 'G',
        'letters': /[\u0047\u24BC\uFF27\u01F4\u011C\u1E20\u011E\u0120\u01E6\u0122\u01E4\u0193\uA7A0\uA77D\uA77E]/g
    },
    { 'base': 'H', 'letters': /[\u0048\u24BD\uFF28\u0124\u1E22\u1E26\u021E\u1E24\u1E28\u1E2A\u0126\u2C67\u2C75\uA78D]/g },
    {
        'base': 'I',
        'letters': /[\u0049\u24BE\uFF29\u00CC\u00CD\u00CE\u0128\u012A\u012C\u0130\u00CF\u1E2E\u1EC8\u01CF\u0208\u020A\u1ECA\u012E\u1E2C\u0197]/g
    },
    { 'base': 'J', 'letters': /[\u004A\u24BF\uFF2A\u0134\u0248]/g },
    { 'base': 'K', 'letters': /[\u004B\u24C0\uFF2B\u1E30\u01E8\u1E32\u0136\u1E34\u0198\u2C69\uA740\uA742\uA744\uA7A2]/g },
    {
        'base': 'L',
        'letters': /[\u004C\u24C1\uFF2C\u013F\u0139\u013D\u1E36\u1E38\u013B\u1E3C\u1E3A\u0141\u023D\u2C62\u2C60\uA748\uA746\uA780]/g
    },
    { 'base': 'LJ', 'letters': /[\u01C7]/g },
    { 'base': 'Lj', 'letters': /[\u01C8]/g },
    { 'base': 'M', 'letters': /[\u004D\u24C2\uFF2D\u1E3E\u1E40\u1E42\u2C6E\u019C]/g },
    {
        'base': 'N',
        'letters': /[\u004E\u24C3\uFF2E\u01F8\u0143\u00D1\u1E44\u0147\u1E46\u0145\u1E4A\u1E48\u0220\u019D\uA790\uA7A4]/g
    },
    { 'base': 'NJ', 'letters': /[\u01CA]/g },
    { 'base': 'Nj', 'letters': /[\u01CB]/g },
    {
        'base': 'O',
        'letters': /[\u004F\u24C4\uFF2F\u00D2\u00D3\u00D4\u1ED2\u1ED0\u1ED6\u1ED4\u00D5\u1E4C\u022C\u1E4E\u014C\u1E50\u1E52\u014E\u022E\u0230\u00D6\u022A\u1ECE\u0150\u01D1\u020C\u020E\u01A0\u1EDC\u1EDA\u1EE0\u1EDE\u1EE2\u1ECC\u1ED8\u01EA\u01EC\u00D8\u01FE\u0186\u019F\uA74A\uA74C]/g
    },
    { 'base': 'OI', 'letters': /[\u01A2]/g },
    { 'base': 'OO', 'letters': /[\uA74E]/g },
    { 'base': 'OU', 'letters': /[\u0222]/g },
    { 'base': 'P', 'letters': /[\u0050\u24C5\uFF30\u1E54\u1E56\u01A4\u2C63\uA750\uA752\uA754]/g },
    { 'base': 'Q', 'letters': /[\u0051\u24C6\uFF31\uA756\uA758\u024A]/g },
    {
        'base': 'R',
        'letters': /[\u0052\u24C7\uFF32\u0154\u1E58\u0158\u0210\u0212\u1E5A\u1E5C\u0156\u1E5E\u024C\u2C64\uA75A\uA7A6\uA782]/g
    },
    {
        'base': 'S',
        'letters': /[\u0053\u24C8\uFF33\u1E9E\u015A\u1E64\u015C\u1E60\u0160\u1E66\u1E62\u1E68\u0218\u015E\u2C7E\uA7A8\uA784]/g
    },
    {
        'base': 'T',
        'letters': /[\u0054\u24C9\uFF34\u1E6A\u0164\u1E6C\u021A\u0162\u1E70\u1E6E\u0166\u01AC\u01AE\u023E\uA786]/g
    },
    { 'base': 'TZ', 'letters': /[\uA728]/g },
    {
        'base': 'U',
        'letters': /[\u0055\u24CA\uFF35\u00D9\u00DA\u00DB\u0168\u1E78\u016A\u1E7A\u016C\u00DC\u01DB\u01D7\u01D5\u01D9\u1EE6\u016E\u0170\u01D3\u0214\u0216\u01AF\u1EEA\u1EE8\u1EEE\u1EEC\u1EF0\u1EE4\u1E72\u0172\u1E76\u1E74\u0244]/g
    },
    { 'base': 'V', 'letters': /[\u0056\u24CB\uFF36\u1E7C\u1E7E\u01B2\uA75E\u0245]/g },
    { 'base': 'VY', 'letters': /[\uA760]/g },
    { 'base': 'W', 'letters': /[\u0057\u24CC\uFF37\u1E80\u1E82\u0174\u1E86\u1E84\u1E88\u2C72]/g },
    { 'base': 'X', 'letters': /[\u0058\u24CD\uFF38\u1E8A\u1E8C]/g },
    {
        'base': 'Y',
        'letters': /[\u0059\u24CE\uFF39\u1EF2\u00DD\u0176\u1EF8\u0232\u1E8E\u0178\u1EF6\u1EF4\u01B3\u024E\u1EFE]/g
    },
    { 'base': 'Z', 'letters': /[\u005A\u24CF\uFF3A\u0179\u1E90\u017B\u017D\u1E92\u1E94\u01B5\u0224\u2C7F\u2C6B\uA762]/g },
    {
        'base': 'a',
        'letters': /[\u0061\u24D0\uFF41\u1E9A\u00E0\u00E1\u00E2\u1EA7\u1EA5\u1EAB\u1EA9\u00E3\u0101\u0103\u1EB1\u1EAF\u1EB5\u1EB3\u0227\u01E1\u00E4\u01DF\u1EA3\u00E5\u01FB\u01CE\u0201\u0203\u1EA1\u1EAD\u1EB7\u1E01\u0105\u2C65\u0250]/g
    },
    { 'base': 'aa', 'letters': /[\uA733]/g },
    { 'base': 'ae', 'letters': /[\u00E6\u01FD\u01E3]/g },
    { 'base': 'ao', 'letters': /[\uA735]/g },
    { 'base': 'au', 'letters': /[\uA737]/g },
    { 'base': 'av', 'letters': /[\uA739\uA73B]/g },
    { 'base': 'ay', 'letters': /[\uA73D]/g },
    { 'base': 'b', 'letters': /[\u0062\u24D1\uFF42\u1E03\u1E05\u1E07\u0180\u0183\u0253]/g },
    { 'base': 'c', 'letters': /[\u0063\u24D2\uFF43\u0107\u0109\u010B\u010D\u00E7\u1E09\u0188\u023C\uA73F\u2184]/g },
    { 'base': 'd', 'letters': /[\u0064\u24D3\uFF44\u1E0B\u010F\u1E0D\u1E11\u1E13\u1E0F\u0111\u018C\u0256\u0257\uA77A]/g },
    { 'base': 'dz', 'letters': /[\u01F3\u01C6]/g },
    {
        'base': 'e',
        'letters': /[\u0065\u24D4\uFF45\u00E8\u00E9\u00EA\u1EC1\u1EBF\u1EC5\u1EC3\u1EBD\u0113\u1E15\u1E17\u0115\u0117\u00EB\u1EBB\u011B\u0205\u0207\u1EB9\u1EC7\u0229\u1E1D\u0119\u1E19\u1E1B\u0247\u025B\u01DD]/g
    },
    { 'base': 'f', 'letters': /[\u0066\u24D5\uFF46\u1E1F\u0192\uA77C]/g },
    {
        'base': 'g',
        'letters': /[\u0067\u24D6\uFF47\u01F5\u011D\u1E21\u011F\u0121\u01E7\u0123\u01E5\u0260\uA7A1\u1D79\uA77F]/g
    },
    {
        'base': 'h',
        'letters': /[\u0068\u24D7\uFF48\u0125\u1E23\u1E27\u021F\u1E25\u1E29\u1E2B\u1E96\u0127\u2C68\u2C76\u0265]/g
    },
    { 'base': 'hv', 'letters': /[\u0195]/g },
    {
        'base': 'i',
        'letters': /[\u0069\u24D8\uFF49\u00EC\u00ED\u00EE\u0129\u012B\u012D\u00EF\u1E2F\u1EC9\u01D0\u0209\u020B\u1ECB\u012F\u1E2D\u0268\u0131]/g
    },
    { 'base': 'j', 'letters': /[\u006A\u24D9\uFF4A\u0135\u01F0\u0249]/g },
    { 'base': 'k', 'letters': /[\u006B\u24DA\uFF4B\u1E31\u01E9\u1E33\u0137\u1E35\u0199\u2C6A\uA741\uA743\uA745\uA7A3]/g },
    {
        'base': 'l',
        'letters': /[\u006C\u24DB\uFF4C\u0140\u013A\u013E\u1E37\u1E39\u013C\u1E3D\u1E3B\u017F\u0142\u019A\u026B\u2C61\uA749\uA781\uA747]/g
    },
    { 'base': 'lj', 'letters': /[\u01C9]/g },
    { 'base': 'm', 'letters': /[\u006D\u24DC\uFF4D\u1E3F\u1E41\u1E43\u0271\u026F]/g },
    {
        'base': 'n',
        'letters': /[\u006E\u24DD\uFF4E\u01F9\u0144\u00F1\u1E45\u0148\u1E47\u0146\u1E4B\u1E49\u019E\u0272\u0149\uA791\uA7A5]/g
    },
    { 'base': 'nj', 'letters': /[\u01CC]/g },
    {
        'base': 'o',
        'letters': /[\u006F\u24DE\uFF4F\u00F2\u00F3\u00F4\u1ED3\u1ED1\u1ED7\u1ED5\u00F5\u1E4D\u022D\u1E4F\u014D\u1E51\u1E53\u014F\u022F\u0231\u00F6\u022B\u1ECF\u0151\u01D2\u020D\u020F\u01A1\u1EDD\u1EDB\u1EE1\u1EDF\u1EE3\u1ECD\u1ED9\u01EB\u01ED\u00F8\u01FF\u0254\uA74B\uA74D\u0275]/g
    },
    { 'base': 'oi', 'letters': /[\u01A3]/g },
    { 'base': 'ou', 'letters': /[\u0223]/g },
    { 'base': 'oo', 'letters': /[\uA74F]/g },
    { 'base': 'p', 'letters': /[\u0070\u24DF\uFF50\u1E55\u1E57\u01A5\u1D7D\uA751\uA753\uA755]/g },
    { 'base': 'q', 'letters': /[\u0071\u24E0\uFF51\u024B\uA757\uA759]/g },
    {
        'base': 'r',
        'letters': /[\u0072\u24E1\uFF52\u0155\u1E59\u0159\u0211\u0213\u1E5B\u1E5D\u0157\u1E5F\u024D\u027D\uA75B\uA7A7\uA783]/g
    },
    {
        'base': 's',
        'letters': /[\u0073\u24E2\uFF53\u00DF\u015B\u1E65\u015D\u1E61\u0161\u1E67\u1E63\u1E69\u0219\u015F\u023F\uA7A9\uA785\u1E9B]/g
    },
    {
        'base': 't',
        'letters': /[\u0074\u24E3\uFF54\u1E6B\u1E97\u0165\u1E6D\u021B\u0163\u1E71\u1E6F\u0167\u01AD\u0288\u2C66\uA787]/g
    },
    { 'base': 'tz', 'letters': /[\uA729]/g },
    {
        'base': 'u',
        'letters': /[\u0075\u24E4\uFF55\u00F9\u00FA\u00FB\u0169\u1E79\u016B\u1E7B\u016D\u00FC\u01DC\u01D8\u01D6\u01DA\u1EE7\u016F\u0171\u01D4\u0215\u0217\u01B0\u1EEB\u1EE9\u1EEF\u1EED\u1EF1\u1EE5\u1E73\u0173\u1E77\u1E75\u0289]/g
    },
    { 'base': 'v', 'letters': /[\u0076\u24E5\uFF56\u1E7D\u1E7F\u028B\uA75F\u028C]/g },
    { 'base': 'vy', 'letters': /[\uA761]/g },
    { 'base': 'w', 'letters': /[\u0077\u24E6\uFF57\u1E81\u1E83\u0175\u1E87\u1E85\u1E98\u1E89\u2C73]/g },
    { 'base': 'x', 'letters': /[\u0078\u24E7\uFF58\u1E8B\u1E8D]/g },
    {
        'base': 'y',
        'letters': /[\u0079\u24E8\uFF59\u1EF3\u00FD\u0177\u1EF9\u0233\u1E8F\u00FF\u1EF7\u1E99\u1EF5\u01B4\u024F\u1EFF]/g
    },
    { 'base': 'z', 'letters': /[\u007A\u24E9\uFF5A\u017A\u1E91\u017C\u017E\u1E93\u1E95\u01B6\u0225\u0240\u2C6C\uA763]/g }
];
class XDefault {
}
XDefault.StrNullDate = "1755-01-01T00:00:00+0000";
XDefault.StrBRNullDate = "01/01/1755 00:00:00";
XDefault.NullDate = new Date(XDefault.StrNullDate);
XDefault.NullID = "00000000-0000-0000-0000-000000000000";
XDefault.DefaultColCount = 32;
XDefault.DefaultRowHeight = 59;
var XEventType;
(function (XEventType) {
    XEventType["MouseMove"] = "mousemove";
    XEventType["MouseDown"] = "mousedown";
    XEventType["MouseUp"] = "mouseup";
    XEventType["MouseEnter"] = "mouseenter";
    XEventType["MouseLeave"] = "mouseleave";
    XEventType["Input"] = "input";
    XEventType["Paste"] = "paste";
    XEventType["KeyDown"] = "keydown";
    XEventType["KeyUp"] = "keyup";
    XEventType["KeyPress"] = "keypress";
    XEventType["LostFocus"] = "focusout";
    XEventType["Click"] = "click";
    XEventType["FocusIn"] = "focusin";
    XEventType["Blur"] = "blur";
    XEventType["Scroll"] = "scroll";
})(XEventType || (XEventType = {}));
class XCallOnce {
    constructor(pUUID, pEvent) {
        this.UUID = pUUID;
        this.Event = pEvent;
    }
    Execute() {
        this.Event.apply(this);
    }
}
class XEventManager {
    static AddExecOnce(pUUID, pEvent) {
        let co = new XCallOnce(pUUID, pEvent);
        XEventManager._CallOnce.Add(co);
    }
    static ExecOnce(pUUID) {
        let co = XEventManager._CallOnce.FirstOrNull(c => c.UUID == pUUID);
        if (co != null) {
            XEventManager._CallOnce.Remove(co);
            co.Execute();
        }
    }
    static AddObserver(pContext, pConfig, pEvent) {
        const observer = new MutationObserver(() => pEvent.apply(pContext));
        observer.observe(pContext.HTML, pConfig);
    }
    static AddEvent(pContext, pElement, pEvent, pMethod, pCheckSource = false) {
        var elm = pElement;
        if (elm.Method == null)
            elm.Method = new Object();
        XEventManager.RemoveEvent(pContext, pElement, pEvent);
        elm.Method[pContext.UUID + "-" + pEvent] = (arg) => {
            XEventManager.Call(pContext, pMethod, pElement, pCheckSource, arg);
        };
        pElement.addEventListener(pEvent, elm.Method[pContext.UUID + "-" + pEvent]);
    }
    static RemoveEvent(pContext, pElement, pEvent) {
        if (pElement.Method != null && pElement.Method[pContext.UUID + "-" + pEvent] != null) {
            pElement.removeEventListener(pEvent, pElement.Method[pContext.UUID + "-" + pEvent]);
            pElement.Method[pContext.UUID + "-" + pEvent] = null;
        }
    }
    static Call(pCallScope, pEvent, pHTM, pCheckSource, pArg) {
        try {
            if (!pCheckSource || pHTM == pArg.srcElement)
                pEvent.apply(pCallScope, [pArg]);
        }
        catch (pError) {
            if (pCallScope.Application != null && pCallScope.Application.ShowError != null)
                pCallScope.Application.ShowError(pError);
            else if (window.ShowError != null)
                window.ShowError(pError);
            else
                throw pError;
        }
    }
    static DelayedEvent(pContext, pEvent, pTime = 100) {
        if (pContext._Timer != null && pContext._Timer != -1)
            window.clearTimeout(pContext._Timer);
        pContext._Timer = setTimeout(() => pEvent.apply(pContext, []), pTime);
    }
    static SetTiemOut(pContext, pEvent, pTime = 100) {
        this.DelayedEvent(pContext, pEvent, pTime);
    }
}
XEventManager._CallOnce = new Array();
class XKeyValue {
}
class Guid {
    static NewGuid() {
        return Guid.NewUUID();
    }
    static IsEmpty(pGuid) {
        return !this.IsFull(pGuid);
    }
    static IsFull(pValue) {
        return !X.IsEmpty(pValue) && pValue.length == 36 && pValue != this.Empty;
    }
    static NewUUID() {
        var d = new Date().getTime();
        var d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now() * 1000)) || 0;
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16;
            if (d > 0) {
                r = (d + r) % 16 | 0;
                d = Math.floor(d / 16);
            }
            else {
                r = (d2 + r) % 16 | 0;
                d2 = Math.floor(d2 / 16);
            }
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16).toUpperCase();
        });
    }
}
Guid.Empty = "00000000-0000-0000-0000-000000000000";
document.Styles = document.styleSheets;
Node.prototype.IsChildOf = function (pElement, pOrIsSelf) {
    var elm = this;
    if (pOrIsSelf && elm == pElement)
        return true;
    while (elm != null) {
        if (elm.parentElement == pElement)
            return true;
        elm = elm.parentElement;
    }
    return false;
};
Node.prototype.Any = function (pPredicate) {
    var elm = this;
    while (elm != null) {
        if (pPredicate(elm))
            return true;
        elm = elm.parentElement;
    }
    return false;
};
Node.prototype.IsChildOf = function (pElement, pOrIsSelf) {
    var elm = this;
    if (pOrIsSelf && elm == pElement)
        return true;
    while (elm != null) {
        if (elm.parentElement == pElement)
            return true;
        elm = elm.parentElement;
    }
    return false;
};
Node.prototype.Any = function (pPredicate) {
    var elm = this;
    while (elm != null) {
        if (pPredicate(elm))
            return true;
        elm = elm.parentElement;
    }
    return false;
};
NodeList.prototype.FirstOrNull = function (pPredicate) {
    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        if (pPredicate == null || pPredicate(item))
            return item;
    }
    return null;
};
Array.prototype.GroupBy = function (pValue) {
    var ar = new Array();
    var ord = this.OrderBy(e => pValue(e));
    var lvlr = null;
    var lar = null;
    for (var i = 0; i < ord.length; i++) {
        let vlr = pValue(ord[i]);
        if (lvlr != vlr) {
            lar = new XKeyValue();
            lar.Key = vlr;
            lar.Value = new XArray();
            ar.Add(lar);
        }
        if (lar != null && lar.Value != null)
            lar.Value.Add(ord[i]);
        lvlr = vlr;
    }
    return ar;
};
Array.prototype.IsEqual = function (pOther, pPredicate) {
    if (pOther == null || this.length != pOther.length)
        return false;
    for (var i = 0; i < this.length; i++)
        if (!pPredicate([this[i], pOther[i]]))
            return false;
    return true;
};
Array.prototype.GetRandom = function (pCount) {
    var ar = this.ToArray();
    if (pCount == null || pCount <= 0)
        pCount = this.length;
    var curidx = Math.min(pCount, ar.length);
    var rndidx = 0;
    XMath.Seed();
    while (0 !== curidx) {
        rndidx = Math.floor(XMath.Random() * curidx);
        curidx--;
        [ar[curidx], ar[rndidx]] = [ar[rndidx], ar[curidx]];
    }
    return ar;
};
Array.prototype.PreviousNOrFirst = function (pN, pPredicate) {
    for (var i = this.length - 1; i >= 0; i--) {
        if ((pPredicate == null || pPredicate(this[i])))
            if (i - pN > 0)
                return this[i - pN];
            else
                return this[0];
    }
    return null;
};
Array.prototype.NextNOrLest = function (pN, pPredicate) {
    for (var i = 0; i < this.length; i++) {
        if ((pPredicate == null || pPredicate(this[i])))
            if (this.length > i + pN)
                return this[i + pN];
            else
                return this[this.length - 1];
    }
    return null;
};
Array.prototype.PreviousFrom = function (pPredicate) {
    for (var i = this.length - 1; i >= 0; i--) {
        var item = this[i];
        if ((pPredicate == null || pPredicate(item)) && i > 0)
            return this[i - 1];
    }
    return null;
};
Array.prototype.NextFrom = function (pPredicate) {
    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        if ((pPredicate == null || pPredicate(item)) && this.length > i + 1)
            return this[i + 1];
    }
    return null;
};
Array.prototype.ToArray = function () {
    var ar = new XArray();
    for (var i = 0; i < this.length; i++)
        ar[i] = this[i];
    return ar;
};
Array.prototype.Assign = function (pArgments) {
    for (var i = 0; i < pArgments.length; i++)
        this[i] = pArgments[i];
};
Array.prototype._ComparerD = function (pLeft, pRigh) {
    var l = this.Order(pLeft);
    var r = this.Order(pRigh);
    if (l.localeCompare)
        return -l.localeCompare(r);
    if (r > l)
        return 1;
    else if (r < l)
        return -1;
    return 0;
};
Array.prototype.FirstOr = function (pPredicate) {
    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        if (pPredicate == null || pPredicate(item))
            return item;
    }
    return null;
};
Array.prototype.ForEach = function (pPredicade) {
    for (var i = 0; i < this.length; i++)
        pPredicade(this[i]);
};
Array.prototype.GetAs = function (pIndex) {
    return this[pIndex];
};
Array.prototype.Any = function (pPredicade) {
    for (var i = 0; i < this.length; i++)
        if (pPredicade(this[i]))
            return true;
    return false;
};
Array.prototype.IndexOf = function (pPredicade) {
    var ar = new XArray();
    for (var i = 0; i < this.length; i++) {
        var data = this[i];
        if (pPredicade(data))
            return i;
    }
    return -1;
};
Array.prototype.SelectDistinct = function (pValue) {
    var ar = new XArray();
    for (var i = 0; i < this.length; i++) {
        let vlr = pValue(this[i]);
        if (!ar.Contains(vlr))
            ar.Add(vlr);
    }
    return ar;
};
Array.prototype.Select = function (pValue) {
    var ar = new XArray();
    for (var i = 0; i < this.length; i++)
        ar.Add(pValue(this[i]));
    return ar;
};
Array.prototype.Where = function (pPredicade) {
    var ar = new XArray();
    for (var i = 0; i < this.length; i++) {
        var data = this[i];
        if (pPredicade(data))
            ar[ar.length] = data;
    }
    return ar;
};
Array.prototype.LPad = function (pLength) {
    var ar = new XArray();
    ar.length = pLength;
    var ed = this.length - 1;
    for (var i = pLength - 1; i >= 0; i--, ed--)
        ar[i] = this[ed];
    return ar;
};
Array.prototype.Delete = function (pStart, pEnd) {
    var p = 0;
    for (var i = 0; i < this.length; i++) {
        if (i >= pStart && i <= pEnd)
            continue;
        this[p++] = this[i];
    }
    this.length = p;
};
Array.prototype.Insert = function (pIndex, pValue) {
    this.length = this.length + 1;
    for (var i = this.length - 1; i >= pIndex; i--)
        this[i] = this[i - 1];
    this[pIndex] = pValue;
};
Array.prototype.FirstBy = function (pValue) {
    var v = null;
    var r = null;
    for (let value of this) {
        var vv = pValue(value);
        if (vv < v || v == null) {
            r = value;
            v = vv;
        }
    }
    return r;
};
Array.prototype.Contains = function (pValue, pStart, pEnd) {
    if (pStart == null || pStart == null) {
        for (var i = 0; i < this.length; i++)
            if (pValue == this[i])
                return true;
    }
    else {
        var e = Math.max(pStart, pEnd);
        var s = Math.min(pStart, pEnd);
        if (e == s)
            return false;
        for (var i = s; i < e; i++) {
            if (i >= 0 && i < this.length)
                if (this[i] == pValue)
                    return true;
        }
    }
    return false;
};
Array.prototype.Count = function (pPredicate) {
    var vlr = 0;
    for (var i = 0; i < this.length; i++) {
        if (pPredicate(this[i]))
            vlr++;
    }
    return vlr;
};
Array.prototype.Sum = function (pPredicate) {
    var vlr = 0;
    for (var i = 0; i < this.length; i++)
        vlr += pPredicate(this[i]);
    return vlr;
};
Array.prototype.Max = function (pPredicate) {
    var vlr = 0;
    for (var i = 0; i < this.length; i++)
        vlr = Math.max(pPredicate(this[i]), vlr);
    return vlr;
};
Array.prototype.Clear = function () {
    this.length = 0;
};
Array.prototype.Add = function (pItem) {
    this[this.length] = pItem;
};
Array.prototype.AddRange = function (pValue) {
    if (X.IsEmpty(pValue))
        return;
    for (var i = 0; i < pValue.length; i++)
        this.Add(pValue[i]);
};
Array.prototype.Remove = function (pItem) {
    var idx = this.indexOf(pItem);
    if (idx >= 0) {
        for (var i = idx; i < this.length - 1; i++)
            this[i] = this[i + 1];
        this.length -= 1;
    }
};
Array.prototype.Get = function (pID) {
    var r = null;
    for (var i = 0; i < this.length; i++)
        if (this[i].ID == pID)
            r = this[i];
    return r;
};
Array.prototype.FirstOrNull = function (pPredicate) {
    for (var i = 0; i < this.length; i++) {
        var item = this[i];
        if (pPredicate == null || pPredicate(item))
            return item;
    }
    return null;
};
Array.prototype.LastOrNull = function (pPredicate) {
    for (var i = this.length - 1; i >= 0; i--) {
        var item = this[i];
        if (pPredicate == null || pPredicate(item))
            return item;
    }
    return null;
};
Array.prototype.OrderBy = function (pOrder) {
    var ar = this.slice();
    ar.Order = pOrder;
    XSort.Sort(ar, XSort.Swap, ar._Comparer, ar);
    return ar;
};
Array.prototype.OrderByEx = function (pOrder) {
    var ar = this.slice();
    ar.Order = pOrder;
    XSort.Sort(ar, XSort.Swap, ar._Comparer, ar);
    return ar;
};
Array.prototype._Comparer = function (pLeft, pRigh) {
    var l = this.Order(pLeft);
    var r = this.Order(pRigh);
    if (l != null && l.localeCompare)
        return l.localeCompare(r);
    if (r > l)
        return -1;
    else if (r < l)
        return 1;
    return 0;
};
HTMLElement.prototype.SetRect = function (pRect) {
    this.style.left = `${pRect.Left}px`;
    this.style.top = `${pRect.Top}px`;
    this.style.width = `${pRect.Width}px`;
    this.style.height = `${pRect.Height}px`;
};
HTMLElement.prototype.StyleStrValue = function (pItemName) {
    var styleValue = "";
    if (document.defaultView && document.defaultView.getComputedStyle)
        styleValue = document.defaultView.getComputedStyle(this, "").getPropertyValue(pItemName);
    //else
    //    if (this.currentStyle)
    //    {
    //        pItemName = pItemName.replace(/\-(\w)/g, (strMatch, p1) => p1.toUpperCase());
    //        styleValue = this.currentStyle[pItemName];
    //    }
    return styleValue;
};
HTMLElement.prototype.StyleValue = function (pItemName) {
    return parseInt(this.StyleStrValue(pItemName));
};
HTMLElement.prototype.GetRect = function (pInternal = false) {
    let or = this.getBoundingClientRect();
    if (pInternal) {
        let r = new XRect(or);
        let bl = this.StyleValue("border-left");
        let bt = this.StyleValue("border-top");
        let br = this.StyleValue("border-right");
        let bb = this.StyleValue("border-bottom");
        if (Number.isNaN(bl) || Number.isNaN(bt) || Number.isNaN(br) || Number.isNaN(bb))
            return r;
        return new XRect(r.Left - bl, r.Top - bt, r.Width - bl - br, r.Height - bt - bb);
    }
    return new XRect(or);
};
HTMLElement.prototype.GetRectRelative = function (pRelative) {
    let or = this.getBoundingClientRect();
    let rr = pRelative.getBoundingClientRect();
    return new XRect(or.left - rr.left, or.top - rr.top, or.width, or.height);
};
HTMLElement.prototype.GetTextWidth = function (pText, pFont) {
    var canvas = window.Canvas || (window.Canvas = document.createElement("canvas"));
    var context = canvas.getContext("2d");
    context.font = pFont;
    var metrics = context.measureText(pText);
    return metrics.width;
};
HTMLElement.prototype.Location = function (pReference) {
    var elm = this;
    var r1 = elm.getBoundingClientRect();
    var r2 = pReference.getBoundingClientRect();
    return new XPoint(r1.left - r2.left, r1.top - r2.top);
};
HTMLElement.prototype.Swap = function (pLeft, pRight) {
    this.insertBefore(this.childNodes[pLeft], this.childNodes[pRight]);
    this.insertBefore(this.childNodes[pRight], this.childNodes[pLeft]);
};
String.prototype.Count = function (pChar) {
    var cnt = 0;
    for (var i = 0; i < this.length; i++)
        if (this[i] == pChar)
            cnt++;
    return cnt;
};
String.prototype.Exchange = function (pPos, pChar) {
    var ret = "";
    for (var i = 0; i < this.length; i++) {
        if (i == pPos)
            ret += pChar;
        else
            ret += this[i];
    }
    return ret;
};
String.prototype.Split = function (pSeparator) {
    var ret = new XArray();
    let prts = this.split(pSeparator);
    for (var i = 0; i < prts.length; i++) {
        let str = prts[i];
        if (X.IsEmpty(prts))
            continue;
        ret[ret.length] = str;
    }
    return ret;
};
String.prototype.LPad = function (pCount, pChar) {
    var str = this == null ? "" : this;
    if (str.length > pCount)
        return str.substr(str.length - pCount, pCount);
    if (X.IsEmpty(pChar))
        pChar = " ";
    return pChar.repeat(pCount - str.length) + str;
};
String.prototype.RPad = function (pCount, pChar) {
    var str = this == null ? "" : this;
    if (str.length > pCount)
        return str.substr(0, pCount);
    if (X.IsEmpty(pChar))
        pChar = " ";
    return str.Add(pChar, pCount - str.length);
};
String.prototype.Add = function (pChar, pCount) {
    var str = (this == null ? "" : this);
    for (var i = 0; i < pCount; i++)
        str = str + pChar;
    return str;
};
String.prototype.ReplaceAll = function (pSearch, pValue) {
    return this.split(pSearch).join(pValue);
};
String.prototype.Exist = function (pValue) {
    return this.indexOf(pValue) != -1;
};
String.prototype.Clean = function () {
    var str = this;
    for (var i = 0; i < Maps.length; i++)
        str = str.replace(Maps[i].letters, Maps[i].base);
    return str;
};
String.prototype.IndexOf = function (pValue) {
    var str = this.Clean();
    pValue = pValue.Clean();
    return str.indexOf(pValue);
};
String.prototype.IsEqual = function (pValue) {
    var str = this;
    if (X.IsEmpty(str) || X.IsEmpty(pValue))
        return false;
    return str == pValue;
};
String.prototype.Contains = function (pValue) {
    if (X.IsEmpty(pValue))
        return false;
    for (var x = 0; x < pValue.length; x++)
        if (this.indexOf(pValue[x].toLowerCase()) == -1)
            return false;
    return true;
};
class XDateTimeResult {
    constructor() {
        this.IsValid = false;
        this.Value = XDefault.NullDate;
        this.IsEmpty = true;
    }
}
var XDatePart;
(function (XDatePart) {
    XDatePart["Year"] = "year";
    XDatePart["Month"] = "month";
    XDatePart["Week"] = "week";
    XDatePart["Day"] = "day";
    XDatePart["Hour"] = "hour";
    XDatePart["Minute"] = "minute";
    XDatePart["Second"] = "second";
})(XDatePart || (XDatePart = {}));
Date.prototype.Add = function (pPart, pValue) {
    var ret = new Date(this);
    var CheckRollover = function (pThis) {
        if (ret.getDate() != pThis.getDate())
            ret.setDate(0);
    };
    switch (pPart) {
        case 'year':
            ret.setFullYear(ret.getFullYear() + pValue);
            CheckRollover(this);
            break;
        case 'month':
            ret.setMonth(ret.getMonth() + pValue);
            CheckRollover(this);
            break;
        case 'week':
            ret.setDate(ret.getDate() + 7 * pValue);
            break;
        case 'day':
            ret.setDate(ret.getDate() + pValue);
            break;
        case 'hour':
            ret.setTime(ret.getTime() + pValue * 3600000);
            break;
        case 'minute':
            ret.setTime(ret.getTime() + pValue * 60000);
            break;
        case 'second':
            ret.setTime(ret.getTime() + pValue * 1000);
            break;
        default:
            ret = this;
            break;
    }
    return ret;
};
Date.prototype.IsToday = function () {
    let now = new Date();
    let it = this;
    return it.getUTCDate() == now.getUTCDate() && it.getUTCMonth() == now.getUTCMonth() && it.getUTCFullYear() == now.getUTCFullYear();
};
Date.prototype.IsLeapYear = function () {
    return Date.IsLeapYear(this.getFullYear());
};
Date.prototype.GetUTCDaysInMonth = function () {
    return Date.GetDaysInMonth(this.getUTCFullYear(), this.getUTCMonth());
};
Date.prototype.AddMonths = function (pValue) {
    var n = this.getUTCDate();
    this.setUTCDate(1);
    this.setUTCMonth(this.getUTCMonth() + pValue);
    this.setUTCDate(Math.min(n, this.GetUTCDaysInMonth()));
    return this;
};
Date.prototype.WeekDay = function () {
    return ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'][this.getDay()];
};
Date.prototype.Month = function () {
    return ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'][this.getMonth()];
};
Date.prototype.LocalDateTimeString = function (pShortY, pShortH, pShowDecimal) {
    return this.LocalDateString(pShortY) + ' ' + this.LocalTimeString(pShortH, pShowDecimal);
};
Date.prototype.DateTimeString = function () {
    return this.DateString() + ' ' + this.TimeString();
};
Date.prototype.LocalTimeString = function (pShort, pShowDecimal) {
    if (pShort)
        return X.PadStart(this.getHours(), 2, "0") + ":" + X.PadStart(this.getMinutes(), 2, "0");
    if (pShowDecimal)
        return X.PadStart(this.getHours(), 2, "0") + ":" + X.PadStart(this.getMinutes(), 2, "0") + ":" + X.PadStart(this.getSeconds(), 2, "0") + "," + X.PadStart(this.getMilliseconds(), 3, "0");
    return X.PadStart(this.getHours(), 2, "0") + ":" + X.PadStart(this.getMinutes(), 2, "0") + ":" + X.PadStart(this.getSeconds(), 2, "0");
};
Date.prototype.LocalDateString = function (pShort) {
    if (pShort)
        return X.PadStart(this.getDate().toString(), 2, "0") + "/" + X.PadStart(this.getMonth() + 1, 2, "0") + "/" + this.getFullYear().toString().LPad(2, '0');
    return X.PadStart(this.getDate().toString(), 2, "0") + "/" + X.PadStart(this.getMonth() + 1, 2, "0") + "/" + this.getFullYear().toString();
};
Date.prototype.DateString = function () {
    var dstr = X.PadStart(this.getUTCDate().toString(), 2, "0") + "/" + X.PadStart(this.getUTCMonth() + 1, 2, "0") + "/" + this.getUTCFullYear().toString();
    return dstr;
};
Date.prototype.TimeString = function (pShort = false) {
    if (!pShort)
        return X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes(), 2, "0") + ":" + X.PadStart(this.getUTCSeconds(), 2, "0");
    return X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes(), 2, "0");
};
Date.prototype.ToLocalISO = function () {
    var dstr = this.getFullYear() + "-" + X.PadStart(this.getMonth() + 1, 2, "0") + "-" + X.PadStart(this.getDate(), 2, "0") + "T" +
        X.PadStart(this.getHours(), 2, "0") + ":" + X.PadStart(this.getMinutes().toString(), 2, "0") + ":" +
        X.PadStart(this.getSeconds(), 2, "0") + "." + X.PadStart(this.getMilliseconds(), 6, "0");
    return dstr;
};
Date.prototype.ToISO = function () {
    var dstr = this.getUTCFullYear() + "-" + X.PadStart(this.getUTCMonth() + 1, 2, "0") + "-" + X.PadStart(this.getUTCDate(), 2, "0") + "T" +
        X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes().toString(), 2, "0") + ":" +
        X.PadStart(this.getUTCSeconds(), 2, "0") + "." + X.PadStart(this.getUTCMilliseconds(), 6, "0");
    return dstr;
};
Date.prototype.OnlyTime = function () {
    var dstr = "1755-01-01T" + X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes().toString(), 2, "0") + ":" +
        X.PadStart(this.getUTCSeconds(), 2, "0") + "." + X.PadStart(this.getUTCMilliseconds(), 6, "0");
    return Date.ToDateTime(dstr, true);
};
Date.prototype.OnlyDate = function () {
    var dstr = this.getUTCFullYear() + "-" + X.PadStart(this.getUTCMonth() + 1, 2, "0") + "-" + X.PadStart(this.getUTCDate(), 2, "0") + "T00:00:00.000000";
    return Date.ToDateTime(dstr, true);
};
Date.prototype.Full = function () {
    var dstr = this.getUTCFullYear() + "-" + X.PadStart(this.getUTCMonth() + 1, 2, "0") + "-" + X.PadStart(this.getUTCDate(), 2, "0") + "T" +
        X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes().toString(), 2, "0") + ":" + X.PadStart(this.getUTCSeconds(), 2, "0") + " " + this.getUTCMilliseconds();
    return dstr;
};
Date.prototype.ToString = function () {
    var dstr = X.PadStart(this.getUTCDate().toString(), 2, "0") + "/" + X.PadStart(this.getUTCMonth() + 1, 2, "0") + "/" + this.getUTCFullYear().toString() + " " +
        X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes(), 2, "0") + ":" + X.PadStart(this.getUTCSeconds(), 2, "0");
    return dstr;
};
Date.prototype.FormatDateTime = function (pPattern) {
    if (this.getFullYear() == 1755)
        return "";
    if (pPattern.length > 16)
        return this.LocalDateTimeString(false, false, true); // 01/01/0001 01:01
    if (pPattern.length == 16)
        return this.LocalDateTimeString(false, true); // 01/01/0001 01:01
    if (pPattern.length == 14)
        return this.LocalDateTimeString(true, true); // 01/01/01 01:01
    if (pPattern.length == 10)
        return this.LocalDateString(); // 01/01/0001
    if (pPattern.length == 8)
        if (pPattern.indexOf(':') == -1)
            return this.LocalDateString(true); // 01/01/01
        else
            return this.LocalTimeString(); // 01:01:01
    if (pPattern.length == 5)
        return this.LocalTimeString(true); // 01:01
    return this.LocalDateTimeString(); // 01/01/0001 01:01:01
};
Date.IsEmpty = function (pValue) {
    if (pValue == null || Date.IsNullDateOrTime(pValue))
        return true;
    return pValue == XDefault.NullDate;
};
Date.IsLeapYear = function (pYear) {
    return (((pYear % 4 === 0) && (pYear % 100 !== 0)) || (pYear % 400 === 0));
};
Date.GetDaysInMonth = function (pYear, pMonth) {
    return [31, (Date.IsLeapYear(pYear) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][pMonth];
};
Date.Parse = function (pValue) {
    var res = new XDateTimeResult();
    if (X.IsEmpty(pValue))
        return res;
    if (Date.IsBRDateTime(pValue))
        res = Date.FromBRDateTime(pValue);
    else if (Date.IsBRDate(pValue))
        res = Date.FromBRDate(pValue);
    else if (Date.IsBRTime(pValue))
        res = Date.FromBRTime(pValue);
    if (Date.IsDateTime(pValue))
        res = Date.FromDateTime(pValue);
    else if (Date.IsDate(pValue))
        res = Date.FromDate(pValue);
    else if (Date.IsTime(pValue))
        res = Date.FromTime(pValue);
    res.IsEmpty = res.IsValid && res.Value.getUTCFullYear() <= 1755;
    return res;
};
Date.IsBRDate = function (pValue) {
    return Date.FromBRDate(pValue).IsValid;
};
Date.FromBRDate = function (pValue) {
    var res = new XDateTimeResult();
    try {
        if (X.IsEmpty(pValue))
            return res;
        pValue = pValue.trim();
        var strs1 = pValue.split(" ");
        if (strs1.length != 1)
            return res;
        var strs2 = strs1[0].split("/");
        if (strs2.length != 3)
            return res;
        var d = new Number(strs2[0]);
        var m = new Number(strs2[1]);
        var y = new Number(strs2[2]);
        if (d < 0 || m <= 0 || y <= 1754)
            return res;
        var iso = strs2[2] + "-" + strs2[1] + "-" + strs2[0] + "T00:00:00+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCDate() != d || dt.getUTCMonth() + 1 != m || dt.getUTCFullYear() != y)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError) {
        return res;
    }
};
Date.IsBRTime = function (pValue) {
    return Date.FromBRTime(pValue).IsValid;
};
Date.FromBRTime = function (pValue) {
    var res = new XDateTimeResult();
    try {
        if (X.IsEmpty(pValue))
            return res;
        var strs1 = pValue.split(" ");
        var strs2;
        if (strs1.length != 2)
            strs2 = pValue.split(":");
        else
            strs2 = strs1[0].split(":");
        if (strs2.length < 2 || strs2.length > 3)
            return res;
        if (strs2.length == 2)
            strs2[2] = "00";
        var h = Number.parseInt(strs2[0]);
        var m = Number.parseInt(strs2[1]);
        var s = Number.parseInt(strs2[2]);
        if (s > 60 || m > 60 || h > 23)
            return res;
        var iso = "1755-01-01T" + strs2[0] + ":" + strs2[1] + ":" + strs2[2] + "+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCHours() != h || dt.getUTCMinutes() != m || dt.getUTCSeconds() != s)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError) {
        return res;
    }
};
Date.IsNullDateOrTime = function (pValue) {
    if (X.IsEmpty(pValue))
        return true;
    if (pValue instanceof Date)
        if (pValue.toJSON() == XDefault.NullDate.toJSON() || pValue.getUTCFullYear() <= 1755)
            return true;
        else
            return false;
    if (!Date.IsDateOrTime(pValue))
        return true;
    if (pValue.indexOf('.') == -1)
        pValue = pValue + "+0000";
    var dt = new Date(pValue);
    return dt.toJSON() == XDefault.NullDate.toJSON() || dt.getUTCFullYear() <= 1755;
};
Date.IsDateOrTime = function (pValue) {
    if (X.IsEmpty(pValue))
        return false;
    if (pValue.getUTCDate)
        return true;
    return Date.IsDate(pValue) || Date.IsTime(pValue) || Date.IsDateTime(pValue) || Date.IsBRDate(pValue) || Date.IsBRTime(pValue) || Date.IsBRDateTime(pValue);
};
Date.ToDateTime = function (pValue, pAsUTC) {
    var ret = null;
    if (Date.IsDateTime(pValue))
        ret = this.FromDateTime(pValue).Value;
    else if (Date.IsTime(pValue))
        ret = this.FromTime(pValue).Value;
    else if (Date.IsDate(pValue))
        ret = this.FromDate(pValue).Value;
    if (Date.IsBRDateTime(pValue))
        ret = this.FromBRDateTime(pValue).Value;
    else if (Date.IsBRDate(pValue))
        ret = this.FromBRDate(pValue).Value;
    else if (Date.IsBRTime(pValue))
        ret = this.FromBRTime(pValue).Value;
    if (ret != null)
        return ret;
    return XDefault.NullDate;
};
Date.IsDate = function (pValue) {
    return this.FromDate(pValue).IsValid;
};
Date.FromDate = function (pValue) {
    var res = new XDateTimeResult();
    try {
        if (X.IsEmpty(pValue))
            return res;
        var strs1 = pValue.split("T");
        if (strs1.length != 2)
            return res;
        var strs2 = strs1[0].split("-");
        if (strs2.length != 3)
            return res;
        var y = Number.parseInt(strs2[0]);
        var m = Number.parseInt(strs2[1]);
        var d = Number.parseInt(strs2[2]);
        if (d < 0 || d > 32 || m < 0 || m > 11 || y <= 1754)
            return res;
        var iso = strs2[0] + "-" + strs2[1] + "-" + strs2[2] + "T00:00:00+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCDate() != d || dt.getUTCMonth() + 1 != m || dt.getUTCFullYear() != y)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError) {
        return res;
    }
};
Date.IsTime = function (pValue) {
    return this.FromTime(pValue).IsValid;
};
Date.FromTime = function (pValue) {
    var res = new XDateTimeResult();
    try {
        if (X.IsEmpty(pValue))
            return res;
        var strs1 = pValue.split(" ");
        var strs2;
        if (strs1.length != 2)
            strs2 = pValue.split(":");
        else
            strs2 = strs1[0].split(":");
        if (strs2.length < 2 || strs2.length > 3)
            return res;
        if (strs2.length == 2)
            strs2[2] = "00";
        var h = Number.parseInt(strs2[0]);
        var m = Number.parseInt(strs2[1]);
        var s = Number.parseInt(strs2[2]);
        if (s > 60 || m > 60 || h > 23)
            return res;
        var iso = "1755-01-01T" + strs2[0] + ":" + strs2[1] + ":" + strs2[2] + "+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCHours() != h || dt.getUTCMinutes() != m || dt.getUTCSeconds() != s)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError) {
        return res;
    }
};
Date.IsDateTime = function (pValue) {
    return this.FromDateTime(pValue).IsValid;
};
Date.FromDateTime = function (pValue) {
    var res = new XDateTimeResult();
    try {
        if (X.IsEmpty(pValue))
            return res;
        var strs1 = pValue.split("T");
        if (strs1.length != 2)
            return res;
        var strs2 = strs1[0].split("-");
        if (strs2.length != 3)
            return res;
        var y = Number.parseInt(strs2[0]);
        var m = Number.parseInt(strs2[1]);
        var d = Number.parseInt(strs2[2]);
        if (d < 0 || d > 31 || m < 0 || m > 12 || y <= 1754)
            return res;
        var strs3 = strs1[1].split(":");
        if (strs3.length < 2 || strs3.length > 3)
            return res;
        if (strs2.length == 2)
            strs3[2] = "00";
        var h = Number.parseInt(strs3[0]);
        var n = Number.parseInt(strs3[1]);
        var s = Number.parseInt(strs3[2]);
        if (s > 60 || n > 60 || h > 23)
            return res;
        var iso = "" + strs2[0] + "-" + strs2[1] + "-" + strs2[2] + "T" + strs3[0] + ":" + strs3[1] + ":" + strs3[2] + "+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCDate() != d || dt.getUTCMonth() + 1 != m || dt.getUTCFullYear() != y || dt.getUTCHours() != h || dt.getUTCMinutes() != n || dt.getUTCSeconds() != s)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError) {
        return res;
    }
};
Date.IsBRDateTime = function (pValue) {
    return this.FromBRDateTime(pValue).IsValid;
};
Date.FromBRDateTime = function (pValue) {
    var res = new XDateTimeResult();
    try {
        if (X.IsEmpty(pValue))
            return res;
        var strs1 = pValue.split(" ");
        if (strs1.length != 2)
            return res;
        var strs2 = strs1[0].split("/");
        if (strs2.length != 3)
            return res;
        var d = Number.parseInt(strs2[0]);
        var m = Number.parseInt(strs2[1]);
        var y = Number.parseInt(strs2[2]);
        if (d < 0 || m <= 0 || y <= 1754)
            return res;
        var strs3 = strs1[1].split(":");
        if (strs3.length < 2 || strs3.length > 3)
            return res;
        if (strs3.length == 2)
            strs3[2] = "00";
        var h = Number.parseInt(strs3[0]);
        var n = Number.parseInt(strs3[1]);
        var s = Number.parseInt(strs3[2]);
        if (s > 60 || n > 60 || h > 23)
            return res;
        var iso = strs2[2] + "-" + strs2[1] + "-" + strs2[0] + "T" + strs3[0] + ":" + strs3[1] + ":" + strs3[2] + "+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCDate() != d || dt.getUTCMonth() + 1 != m || dt.getUTCFullYear() != y || dt.getUTCHours() != h || dt.getUTCMinutes() != n || dt.getUTCSeconds() != s)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError) {
        return res;
    }
};
class X {
    static DataIsEmpty(pValue) {
        return X.IsEmpty(pValue) || ["NI", "NA"].Contains(pValue);
    }
    static AddNL(pSource, ...pValues) {
        if (!X.IsEmpty(pSource))
            return pValues + "\r\n" + pValues.join("\r\n");
        return pValues.join("\r\n");
    }
    static Lower(pString) {
        if (X.IsEmpty(pString))
            return pString;
        return pString.toString().toLowerCase();
    }
    static Split(pValue, pSeparetor) {
        if (X.IsEmpty(pValue))
            return [];
        var prts = pValue.split(pSeparetor);
        var ret = new Array();
        for (var i = 0; i < prts.length; i++) {
            var prt = prts[i];
            if (!X.IsEmpty(prt))
                prt = prt.trim();
            if (X.IsEmpty(prt))
                continue;
            ret.Add(prt);
        }
        return ret;
    }
    static In(pValue, ...pValues) {
        for (var i = 0; i < pValues.length; i++)
            if (pValue == pValues[i])
                return true;
        return false;
    }
    static Exists(pData, ...pValues) {
        if (X.IsEmpty(pData) || X.IsEmpty(pValues))
            return false;
        for (var i = 0; i < pValues.length; i++) {
            if (pData.IndexOf(pValues[i]) != -1)
                return true;
        }
        return false;
    }
    static ToDate(pValue) {
        return new Date(pValue);
    }
    static IsNumber(pValue) {
        if (X.IsEmpty(pValue))
            return false;
        return !isNaN(Number(pValue.toString()));
    }
    static IsF5(pArg) {
        return ((pArg.which || pArg.keyCode) == XKey.K_F5);
    }
    static IsAlpha(pValue) {
        return pValue >= "A" && pValue <= "Z" || pValue >= "a" && pValue <= "z";
    }
    static IsNum(pValue) {
        return pValue >= "0" && pValue <= "9";
    }
    static Sleep(pTime) {
        var start = new Date().getTime();
        for (var i = 0; i < 1e7; i++)
            if ((new Date().getTime() - start) > pTime)
                break;
    }
    static Length(pValue) {
        if (pValue != null && pValue.length)
            return pValue.length;
        return -1;
    }
    static PadStart(pString, pSize, pAdd) {
        pString = pString.toString();
        if (pString.padStart)
            return pString.padStart(pSize, pAdd);
        if (X.IsEmpty(pAdd) || pAdd == "undefined")
            pAdd = ' ';
        if (X.IsEmpty(pString))
            pString = " ";
        else
            pString = pString.toString();
        if (pString.length < pSize)
            pString = pAdd.repeat(pSize + 1) + pString;
        return pString.substring(pString.length - pSize, pString.length);
    }
    static IfNull(pString, pValue) {
        if (X.IsEmpty(pString))
            return pValue;
        return pString;
    }
    static As(pValue) {
        return pValue;
    }
    static Void(pArg) {
        return false;
    }
    static IsChar(pValue) {
        if (X.IsEmpty(pValue) && pValue == " ")
            return false;
        return pValue.length == 1 && (pValue == " " || (pValue >= "0" && pValue <= "9") || (pValue >= "A" && pValue <= "Z") || (pValue >= "a" && pValue <= "z"));
    }
    static IsEmpty(pValue) {
        if (pValue == Guid.Empty)
            return true;
        return pValue == null || pValue == "undefined" || pValue.toString() == "" || (pValue.length != null && (pValue.length == 0 || pValue == " ".repeat(pValue.length)));
    }
    static Contains(pArray, pValue) {
        return !X.IsEmpty(pArray) && pArray.Contains(pValue);
    }
}
class XCall {
    static AddEvent(pContext, pElement, pEvent, pMethod) {
        if (pElement.Method == null)
            pElement.Method = new Object();
        pElement.Method[pContext.UUID + pEvent] = (arg) => XCall.Call(pContext, pMethod, [pElement]);
        pElement.addEventListener(pEvent, pElement.Method[pContext.UUID + pEvent]);
    }
    static RemoveEvent(pContext, pElement, pEvent) {
        if (pElement.Method != null && pElement.Method[pContext.UUID + pEvent] != null)
            pElement.removeEventListener(pEvent, pElement.Method[pContext.UUID + pEvent]);
    }
    static RemoveAll(pElement) {
        if (pElement.Method != null)
            for (var vle in pElement.Method)
                pElement.removeEventListener(vle, pElement.Method[vle]);
    }
    static Call(pCallScope, pEvent, pArg) {
        try {
            pEvent.apply(pCallScope, pArg);
        }
        catch (pError) {
            if (pCallScope.Application != null)
                if (pCallScope.Application.ShowError == null)
                    throw (pError);
                else
                    pCallScope.Application.ShowError(pError);
            else if (window.ShowError != undefined)
                window.ShowError(pError);
            else
                throw (pError);
        }
    }
}
class XHotkeyManager {
    static OnKeyDown(pArg) {
        let elm = pArg.target;
        if (!X.IsEmpty(pArg.key) && pArg.key.length == 1 && pArg.altKey) {
            pArg.preventDefault();
            return false;
        }
        return true;
    }
}
class XMath {
    //static AddCorner(pCorner: XPoint, pRound: number, pOut1: XPoint, pOut2: XPoint): XArray<XPoint>
    //{
    //    if (!pCorner.Equals(pOut1) && !pCorner.Equals(pOut2) && ((pOut1.Y == pCorner.Y && pCorner.X == pOut2.X) || (pOut1.X == pCorner.X && pCorner.Y == pOut2.Y)))
    //    {
    //        let x1 = pCorner.X == pOut1.X ? pRound * 2 : Math.abs(pCorner.X - pOut1.X);
    //        let y1 = pCorner.Y == pOut1.Y ? pRound * 2 : Math.abs(pCorner.Y - pOut1.Y);
    //        let x2 = pCorner.X == pOut2.X ? pRound * 2 : Math.abs(pCorner.X - pOut2.X);
    //        let y2 = pCorner.Y == pOut2.Y ? pRound * 2 : Math.abs(pCorner.Y - pOut2.Y);
    //        let size = Math.min(Math.min(x1, y1), Math.min(x2, y2));
    //        pRound = size / 2;
    //    }
    //    else
    //        pRound = 0;
    //    pOut1 = XMath.PointCircle(pCorner, pOut1, pRound);
    //    pOut2 = XMath.PointCircle(pCorner, pOut2, pRound);
    //    return [pOut1, pCorner, pOut2];
    //}
    static CreateArrow(pt, pt2, pWidth) {
        let dg = this.AngleInRad(pt, pt2) / Math.PI * 180 - 90;
        let ln1 = XMath.RotatePoints(pt, [pt, new XPoint(pt.X + pWidth, pt.Y)], dg - 22.5);
        let ln2 = XMath.RotatePoints(pt, ln1, 45);
        return [ln1[0], ln1[1], ln2[1], ln2[0]];
    }
    static RotatePoints(pCenter, pPoints, pDegree) {
        let ret = new XArray();
        for (var i = 0; i < pPoints.length; i++)
            ret.Add(XMath.RotatePoint(pCenter, pPoints[i], pDegree));
        return ret;
    }
    static RotatePoint(pCenter, pPoint, pDegree) {
        let length = XMath.Distance2Points(pPoint, pCenter);
        let degree = XMath.AngleInRad(pPoint, pCenter) + ((pDegree * Math.PI) / 180);
        return new XPoint(pCenter.X - (length * Math.sin(degree)), pCenter.Y - (length * Math.cos(degree)));
    }
    static Round(pRect, pFactor) {
        return new XRect(XMath.RoundN(pRect.Left, pFactor), XMath.RoundN(pRect.Top, pFactor), XMath.RoundN(pRect.Width, pFactor), XMath.RoundN(pRect.Height, pFactor));
    }
    static RoundN(pValue, pFactor) {
        if (Number.isNaN(pValue))
            return 0;
        var v = Math.floor(pValue / pFactor);
        return v * pFactor;
    }
    static Distance2Points(pPoint, pCenter) {
        return Math.sqrt(Math.pow(pPoint.X - pCenter.X, 2) + Math.pow(pPoint.Y - pCenter.Y, 2));
    }
    static LineIntersectsRect(pRect, p1, p2) {
        return XMath.LineIntersectsLine(p1, p2, new XPoint(pRect.X, pRect.Y), new XPoint(pRect.X + pRect.Width, pRect.Y)) ||
            XMath.LineIntersectsLine(p1, p2, new XPoint(pRect.X + pRect.Width, pRect.Y), new XPoint(pRect.X + pRect.Width, pRect.Y + pRect.Height)) ||
            XMath.LineIntersectsLine(p1, p2, new XPoint(pRect.X + pRect.Width, pRect.Y + pRect.Height), new XPoint(pRect.X, pRect.Y + pRect.Height)) ||
            XMath.LineIntersectsLine(p1, p2, new XPoint(pRect.X, pRect.Y + pRect.Height), new XPoint(pRect.X, pRect.Y)) ||
            (pRect.Contains(p1) && pRect.Contains(p2));
    }
    static LineIntersectsLine(l1p1, l1p2, l2p1, l2p2) {
        var q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
        var d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);
        if (d == 0)
            return false;
        var r = q / d;
        q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
        var s = q / d;
        if (r < 0 || r > 1 || s < 0 || s > 1)
            return false;
        return true;
    }
    static LineIntersection(pP1Line1, pP2Line1, pP1Line2, pP2Line2) {
        var dx1 = pP2Line1.X - pP1Line1.X;
        var dy1 = pP2Line1.Y - pP1Line1.Y;
        var dx2 = pP2Line2.X - pP1Line2.X;
        var dy2 = pP2Line2.Y - pP1Line2.Y;
        var det = (dx2 * dy1) - (dy2 * dx1);
        if (det == 0)
            return new XPoint(Number.NaN, Number.NaN);
        var mu = (((pP1Line1.X - pP1Line2.X) * dy1) - ((pP1Line1.Y - pP1Line2.Y) * dx1)) / det;
        var mu2 = (((pP1Line1.X - pP1Line2.X) * dy2) - ((pP1Line1.Y - pP1Line2.Y) * dx2)) / det;
        if (mu >= 0 && mu <= 1 && mu2 >= 0 && mu2 <= 1)
            return new XPoint(pP1Line2.X + (mu * dx2), pP1Line2.Y + (mu * dy2));
        return new XPoint(Number.NaN, Number.NaN);
    }
    static ToPolygonEx(pRect, pInflateLine = 0) {
        var list = new XArray();
        list[0] = [new XPoint(pRect.Left - pInflateLine, pRect.Top), new XPoint(pRect.Right + pInflateLine, pRect.Top)];
        list[1] = [new XPoint(pRect.Right, pRect.Top - pInflateLine), new XPoint(pRect.Right, pRect.Bottom + pInflateLine)];
        list[2] = [new XPoint(pRect.Right + pInflateLine, pRect.Bottom), new XPoint(pRect.Left - pInflateLine, pRect.Bottom)];
        list[3] = [new XPoint(pRect.Left, pRect.Bottom + pInflateLine), new XPoint(pRect.Left, pRect.Top - pInflateLine)];
        return list;
    }
    static AddCorner(pCorner, pRound, pP1, pP2) {
        if (!pCorner.Equals(pP1) && !pCorner.Equals(pP2) && ((Math.floor(pP1.Y) == Math.floor(pCorner.Y) && Math.floor(pCorner.X) == Math.floor(pP2.X)) ||
            (Math.floor(pP1.X) == Math.floor(pCorner.X) && Math.floor(pCorner.Y) == Math.floor(pP2.Y)))) {
            var x1 = pCorner.X == pP1.X ? pRound * 2 : Math.abs(pCorner.X - pP1.X);
            var y1 = pCorner.Y == pP1.Y ? pRound * 2 : Math.abs(pCorner.Y - pP1.Y);
            var x2 = pCorner.X == pP2.X ? pRound * 2 : Math.abs(pCorner.X - pP2.X);
            var y2 = pCorner.Y == pP2.Y ? pRound * 2 : Math.abs(pCorner.Y - pP2.Y);
            var size = Math.min(Math.min(x1, y1), Math.min(x2, y2));
            pRound = size / 2;
        }
        else
            pRound = 0;
        return [XMath.PointCircle(pCorner, pP1, pRound), pCorner, XMath.PointCircle(pCorner, pP2, pRound)];
    }
    static PointCircle(pCenter, pPoint, pRadiusX, pRadiusY = -1) {
        if (pRadiusY == -1)
            pRadiusY = pRadiusX;
        var dg = XMath.AngleInRad(pCenter, pPoint) + Math.PI;
        return new XPoint(pCenter.X - (pRadiusX * Math.sin(dg)), pCenter.Y - (pRadiusY * Math.cos(dg)));
    }
    static AngleInRad(pFirst, pSecond) {
        var degree = 0;
        if (pFirst.X == pSecond.X)
            if (pFirst.Y < pSecond.Y)
                degree = Math.PI * 1.5;
            else
                degree = Math.PI / 2.0;
        else
            degree = Math.atan((pSecond.Y - pFirst.Y) / (pFirst.X - pSecond.X));
        if (pSecond.X < pFirst.X)
            degree = degree + Math.PI;
        degree = degree + (Math.PI / 2.0);
        return degree;
    }
    static PolarToCartesian(pCenter, pRadius, pDegrees) {
        var rad = (pDegrees - 90) * Math.PI / 180.0;
        return new XPoint(pCenter.X + (pRadius * Math.cos(rad)), pCenter.Y + (pRadius * Math.sin(rad)));
    }
    static DonutSlice(pCenter, pRadius, pStartDegrees, pEndDegrees, pWidth) {
        var start = XMath.PolarToCartesian(pCenter, pRadius, pEndDegrees);
        var end = XMath.PolarToCartesian(pCenter, pRadius, pStartDegrees);
        var iradius = pRadius - pWidth;
        var istart = XMath.PolarToCartesian(pCenter, iradius, pEndDegrees);
        var iend = XMath.PolarToCartesian(pCenter, iradius, pStartDegrees);
        var flag = pEndDegrees - pStartDegrees <= 180 ? "0" : "1";
        var x = "";
        var d = ["M", start.X, start.Y, "A", pRadius, pRadius, 0, flag, 0, end.X, end.Y, "L", iend.X, iend.Y, "A", iradius, iradius, 0, flag, 1, istart.X, istart.Y, "Z", x].join(" ");
        return d;
    }
    static PieSlice(pCenter, pRadius, pStartDegrees, pEndDegrees) {
        var start = XMath.PolarToCartesian(pCenter, pRadius, pEndDegrees);
        var end = XMath.PolarToCartesian(pCenter, pRadius, pStartDegrees);
        var flag = pEndDegrees - pStartDegrees <= 180 ? "0" : "1";
        var d = ["M", pCenter.X, pCenter.Y, "L", start.X, start.Y, "A", pRadius, pRadius, 0, flag, 0, end.X, end.Y, "L", pCenter.X, pCenter.Y].join(" ");
        return d;
    }
    static Arc(pCenter, pRadius, pStartDegrees, pEndDegrees) {
        var start = XMath.PolarToCartesian(pCenter, pRadius, pEndDegrees);
        var end = XMath.PolarToCartesian(pCenter, pRadius, pStartDegrees);
        var flag = pEndDegrees - pStartDegrees <= 180 ? "0" : "1";
        var d = ["M", start.X, start.Y, "A", pRadius, pRadius, 0, flag, 0, end.X, end.Y].join(" ");
        return d;
    }
    static Seed(pSeed = -1) {
        if (pSeed == -1)
            pSeed = new Date().getTime();
        XMath.m_w = (123456789 + pSeed) & XMath.mask;
        XMath.m_z = (987654321 - pSeed) & XMath.mask;
    }
    static Random() {
        XMath.m_z = (36969 * (XMath.m_z & 65535) + (XMath.m_z >> 16)) & XMath.mask;
        XMath.m_w = (18000 * (XMath.m_w & 65535) + (XMath.m_w >> 16)) & XMath.mask;
        var result = ((XMath.m_z << 16) + (XMath.m_w & 65535)) >>> 0;
        result /= 4294967296;
        return result;
    }
}
XMath.m_w = 123456789;
XMath.m_z = 987654321;
XMath.mask = 0xffffffff;
class XPopupManager {
    static ZIndex() {
        this._ZIndex++;
        return this._ZIndex.toString();
    }
    static AddAutoEvent(pContext, pMethod, pOnce = true) {
        var obj = { Context: pContext, Method: pMethod, Once: pOnce };
        this.AutoEvent.Add(obj);
    }
    static Remove(pView) {
        XPopupManager.PopupList.Remove(pView);
    }
    static Show(pView) {
        pView.Show();
        //pView.HTML.scrollIntoView();
    }
    static Add(pView) {
        XPopupManager.PopupList.Add(pView);
    }
    static HideAll(pArg, pValid = false) {
        if (pArg != null && this.UseCrl && !pArg.ctrlKey)
            return;
        var ar = XPopupManager.AutoEvent.ToArray();
        for (var i = 0; i < ar.length; i++) {
            var m = ar[i];
            if (pArg != null && !m.Context.CanClose(pArg.srcElement))
                continue;
            m.Method.apply(m.Context);
            if (m.Once)
                XPopupManager.AutoEvent.Remove(m);
        }
        for (var i = 0; i < XPopupManager.PopupList.length; i++) {
            var elm = XPopupManager.PopupList[i];
            if (!elm.IsVisible)
                continue;
            if (pArg == null || elm.CanClose(pArg.srcElement)) {
                if (!pValid)
                    elm.CallPopupClosed();
                elm.IsVisible = false;
            }
        }
    }
}
XPopupManager._ZIndex = 999;
XPopupManager.PopupList = new Array();
XPopupManager.AutoEvent = new Array();
XPopupManager.UseCrl = false;
class XSort {
    static Sort(pArray, pSwap, pComparer, pOwner) {
        XSort.QuickSort(pArray, 0, pArray.length - 1, pSwap, pComparer, pOwner);
        return pArray;
    }
    static QuickSort(map, left, right, pSwap, pComparer, pOwner) {
        do {
            var i = left;
            var j = right;
            var elm = map[i + ((j - i) >> 1)];
            do {
                while (i < map.length && pComparer.call(pOwner, elm, map[i]) > 0)
                    i++;
                while (j >= 0 && pComparer.call(pOwner, elm, map[j]) < 0)
                    j--;
                if (i > j)
                    break;
                if (i < j)
                    pSwap.call(pOwner, map, i, j);
                i++;
                j--;
            } while (i <= j);
            if (j - left <= right - i) {
                if (left < j)
                    XSort.QuickSort.call(pOwner, map, left, j, pSwap, pComparer, pOwner);
                left = i;
            }
            else {
                if (i < right)
                    XSort.QuickSort.call(pOwner, map, i, right, pSwap, pComparer, pOwner);
                right = j;
            }
        } while (left < right);
    }
    static Swap(pArray, pLeft, pRight) {
        var tmp = pArray[pLeft];
        pArray[pLeft] = pArray[pRight];
        pArray[pRight] = tmp;
    }
}
class XUtils {
    static SetCursor(pElement, pType) {
        switch (pType) {
            case XDragType.LeftTop:
                pElement.style.cursor = "nw-resize;";
                break;
            case XDragType.Top:
                pElement.style.cursor = "n-resize";
                break;
            case XDragType.RightTop:
                pElement.style.cursor = "ne-resize";
                break;
            case XDragType.Right:
                pElement.style.cursor = "e-resize";
                break;
            case XDragType.RightBottom:
                pElement.style.cursor = "se-resize";
                break;
            case XDragType.Bottom:
                pElement.style.cursor = "s-resize";
                break;
            case XDragType.LeftBottom:
                pElement.style.cursor = "sw-resize";
                break;
            case XDragType.Left:
                pElement.style.cursor = "w-resize";
                break;
            case XDragType.Drag:
                pElement.style.cursor = "move";
                break;
            default:
                pElement.style.cursor = "default";
                break;
        }
    }
    static Location(pElement) {
        var prect = null;
        if (pElement.parentElement != null)
            prect = pElement.parentElement.getBoundingClientRect();
        var rect = pElement.getBoundingClientRect();
        if (prect != null)
            return new XPoint(rect.left - prect.left, rect.top - prect.top);
        return new XPoint(rect.left, rect.top);
    }
    static IsOut(pRect, pLocation, pWidth, pHeight) {
        return (pLocation.IsLessZero || (pRect.width < pWidth + pLocation.X) || (pRect.height < pHeight + pLocation.Y));
    }
    static IsNumber(pValue) {
        return !isNaN(parseFloat(pValue)) && isFinite(pValue);
    }
    static AddElement(pOwner, pTag, pClass = null, pInsert = false) {
        if (pTag == null)
            throw new Error(`Parameter "pTag" can�t be null`);
        var own;
        if (pOwner == null)
            own = document.body;
        else if (pOwner instanceof HTMLElement)
            own = pOwner;
        else
            own = pOwner.HTML;
        var elm = document.createElement(pTag);
        if (pClass != null)
            elm.className = pClass;
        if (pInsert && own.childNodes.length > 0)
            own.insertBefore(elm, elm.childNodes[0]);
        else
            own.appendChild(elm);
        if (pOwner == null)
            elm.Owner = pOwner;
        else if (pOwner instanceof XElement)
            elm.Owner = pOwner;
        return elm;
    }
}
/// <reference path="Utils/XUtils.ts" />
class XSize {
    constructor(pWidth = 0, pHeight = 0) {
        this.Width = pWidth;
        this.Height = pHeight;
    }
    Equal(pOther) {
        return pOther != null && pOther.Width == this.Width && pOther.Height == this.Height;
    }
}
class XArray extends Array {
    constructor(pArg) {
        super();
        if (pArg != null) {
            if (pArg.length > 0) {
                for (var i = 0; i < pArg.length; i++)
                    this[i] = pArg[i];
            }
            else {
                if (pArg > 0) {
                    this.length = pArg;
                    for (var i = 0; i < this.length; i++)
                        this[i] = null;
                }
            }
        }
    }
}
class XHashSet {
    constructor() {
        this.Items = new Object();
        this.List = new XArray();
    }
    get Count() {
        if (this.List == null)
            return 0;
        return this.List.length;
    }
    Add(pItem, pID) {
        this.Items[pID] = pItem;
        this.List.Add(pID);
        return pItem;
    }
    Contains(pID) {
        return this.Items[pID] != null;
    }
    Get(pID) {
        return this.Items[pID];
    }
    Remove(pID) {
        for (var i = 0; i < this.List.length; i++) {
            var v = this.List[i];
            if (v.ID == pID) {
                this.List.Remove(v);
                break;
            }
        }
        this.Items[pID] = null;
    }
}
var XAction;
(function (XAction) {
    XAction[XAction["Save"] = 1] = "Save";
    XAction[XAction["Close"] = 2] = "Close";
    XAction[XAction["NewTuple"] = 3] = "NewTuple";
})(XAction || (XAction = {}));
var XDragType;
(function (XDragType) {
    XDragType[XDragType["LeftTop"] = 0] = "LeftTop";
    XDragType[XDragType["Top"] = 1] = "Top";
    XDragType[XDragType["RightTop"] = 2] = "RightTop";
    XDragType[XDragType["Right"] = 3] = "Right";
    XDragType[XDragType["RightBottom"] = 4] = "RightBottom";
    XDragType[XDragType["Bottom"] = 5] = "Bottom";
    XDragType[XDragType["LeftBottom"] = 6] = "LeftBottom";
    XDragType[XDragType["Left"] = 7] = "Left";
    XDragType[XDragType["Drag"] = 8] = "Drag";
    XDragType[XDragType["Error"] = 9] = "Error";
})(XDragType || (XDragType = {}));
class XHSLColor {
    constructor(pH, pS, pL) {
        this.A = 1;
        this.H = pH;
        this.S = pS;
        this.L = pL;
    }
    get RGB() { return XHSLColor.HSLToRGB(this.H, this.S, this.L, this.A); }
    static StringToRGB(pColor) {
        var c;
        if (/^#([A-Fa-f0-9]{3}){1,2}$/.test(pColor)) {
            c = pColor.substring(1).split('');
            if (c.length == 3) {
                c = [c[0], c[0], c[1], c[1], c[2], c[2]];
            }
            c = '0x' + c.join('');
            return [(c >> 16) & 255, (c >> 8) & 255, c & 255];
        }
        return [0, 0, 0];
    }
    static RGBToHSL(pR, pG, pB) {
        pR /= 255, pG /= 255, pB /= 255;
        var max = Math.max(pR, pG, pB);
        var min = Math.min(pR, pG, pB);
        var h = 0;
        var s = 0;
        var l = (max + min) / 2;
        if (max == min)
            h = s = 0;
        else {
            var d = max - min;
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
            switch (max) {
                case pR:
                    h = (pG - pB) / d + (pG < pB ? 6 : 0);
                    break;
                case pG:
                    h = (pB - pR) / d + 2;
                    break;
                case pB:
                    h = (pR - pG) / d + 4;
                    break;
            }
            h /= 6;
        }
        return new XHSLColor(h, s, l);
    }
    static HSLToRGB(pH, pS, pL, pA) {
        var r = 0;
        var g = 0;
        var b = 0;
        if (pS == 0)
            r = g = b = pL;
        else {
            var hue2rgb = function hue2rgb(p, q, t) {
                if (t < 0)
                    t += 1;
                if (t > 1)
                    t -= 1;
                if (t < 1 / 6)
                    return p + (q - p) * 6 * t;
                if (t < 1 / 2)
                    return q;
                if (t < 2 / 3)
                    return p + (q - p) * (2 / 3 - t) * 6;
                return p;
            };
            var q = pL < 0.5 ? pL * (1 + pS) : pL + pS - pL * pS;
            var p = 2 * pL - q;
            r = hue2rgb(p, q, pH + 1 / 3);
            g = hue2rgb(p, q, pH);
            b = hue2rgb(p, q, pH - 1 / 3);
        }
        return "#" + Math.round(r * 255).toString(16).LPad(2, '0') + Math.round(g * 255).toString(16).LPad(2, '0') + Math.round(b * 255).toString(16).LPad(2, '0') + Math.round(pA * 255).toString(16).LPad(2, '0');
    }
}
class XPoint {
    static get Empty() {
        return this._Empty;
    }
    constructor(pX = Number.NaN, pY = Number.NaN) {
        this.X = pX;
        this.Y = pY;
    }
    get IsLessZero() {
        return this.X < 0 || this.Y < 0;
    }
    Equals(pOther) {
        return this.X == pOther.X && this.Y == pOther.Y;
    }
    LocationType(pW, pH, pSize = 4, pDragArea = 35) {
        var x = this.X;
        var y = this.Y;
        if (x <= pSize && y <= pSize)
            return XDragType.LeftTop;
        else if (x >= pW - pSize && y <= pSize)
            return XDragType.RightTop;
        else if (x >= pW - pSize && y >= pH - pSize)
            return XDragType.RightBottom;
        else if (x <= pSize && y >= pH - pSize)
            return XDragType.LeftBottom;
        else if (y <= pSize)
            return XDragType.Top;
        else if (x >= pW - pSize)
            return XDragType.Right;
        else if (y >= pH - pSize)
            return XDragType.Bottom;
        else if (x <= pSize)
            return XDragType.Left;
        else if (y > pSize && y <= pDragArea)
            return XDragType.Drag;
        return XDragType.Error;
    }
    AsString() {
        return this.X + " " + this.Y;
    }
    toString() {
        return "X=" + this.X + " Y=" + this.Y;
    }
}
XPoint._Empty = new XPoint(0, 0);
class XRect {
    static get Empty() {
        return this._Empty;
    }
    static FromPoints(pLeftTop, pRightBottom) {
        return new XRect(pLeftTop.X, pLeftTop.Y, pRightBottom.X - pLeftTop.X, pRightBottom.Y - pLeftTop.Y);
    }
    constructor(pLeft = 0, pTop = 0, pWidth = 0, pHeight = 0) {
        this.Left = 0;
        this.Top = 0;
        this.Width = 0;
        this.Height = 0;
        this.Bottom = 0;
        this.Right = 0;
        if (pLeft instanceof DOMRect) {
            let r = pLeft;
            pLeft = r.left;
            pTop = r.top;
            pWidth = r.width;
            pHeight = r.height;
        }
        if (!XUtils.IsNumber(pLeft)) {
            var pts = pLeft.split(';');
            pLeft = Number.parseInt(pts[0]);
            pTop = Number.parseInt(pts[1]);
            pWidth = Number.parseInt(pts[2]);
            pHeight = Number.parseInt(pts[3]);
        }
        this.SetValue(pLeft, pTop, pWidth, pHeight);
    }
    get IsEmpty() { return this.Width <= 0 || this.Height <= 0; }
    get LeftTop() { return new XPoint(this.Left, this.Top); }
    get RightTop() { return new XPoint(this.Left + this.Width, this.Top); }
    get LeftBottom() { return new XPoint(this.Left, this.Top + this.Height); }
    get RightBottom() { return new XPoint(this.Left + this.Width, this.Top + this.Height); }
    get X() { return this.Left; }
    get Y() { return this.Top; }
    get AsPath() {
        var d = ["M", this.Left, this.Top, "L", this.Right, this.Top, this.Right, this.Bottom, this.Left, this.Bottom, this.Left, this.Top, "Z"].join(" ");
        return d;
    }
    toString() {
        return [this.Left, this.Top, this.Width, this.Height].join(" ");
    }
    IntersectsWith(pRect) {
        if (this.IsEmpty || pRect.IsEmpty)
            return false;
        return (pRect.Left <= this.Right) && (pRect.Right >= this.Left) && (pRect.Top <= this.Bottom) && (pRect.Bottom >= this.Top);
    }
    Clone() {
        return new XRect(this.Left, this.Top, this.Width, this.Height);
    }
    ApplyStyle(pStyle) {
        pStyle.left = this.Left + "px";
        pStyle.top = this.Top + "px";
        pStyle.width = this.Width + "px";
        pStyle.height = this.Height + "px";
    }
    Union(pRect) {
        if (this.IsEmpty) {
            this.SetValue(pRect.Left, pRect.Top, pRect.Width, pRect.Height);
            return;
        }
        var l = Math.min(this.Left, pRect.Left);
        var t = Math.min(this.Top, pRect.Top);
        var w = Math.max(this.Right, pRect.Right) - l;
        var h = Math.max(this.Bottom, pRect.Bottom) - t;
        this.SetValue(l, t, w, h);
    }
    SetValue(pLeft, pTop, pWidth, pHeight) {
        this.Left = pLeft;
        this.Top = pTop;
        this.Width = pWidth;
        this.Height = pHeight;
        this.Bottom = pTop + pHeight;
        this.Right = pLeft + pWidth;
        this.Size = new XSize(pWidth, pHeight);
    }
    Inflate(pWidth, pHeight) {
        var l = this.Left - pWidth;
        var t = this.Top - pHeight;
        var w = this.Width + pWidth * 2;
        var h = this.Height + pHeight * 2;
        this.SetValue(l, t, w, h);
    }
    AsSelectPath(pValue = 2) {
        var d = ["M", this.Left, this.Top, "L", this.Right, this.Top, this.Right, this.Bottom - pValue, this.Left, this.Bottom - pValue, "Z"].join(" ");
        return d;
    }
    Center() {
        return new XPoint(this.Left + (this.Width / 2), this.Top + (this.Height / 2));
    }
    Contains(pPoint) {
        return ((pPoint.X >= this.Left) && (pPoint.X - this.Width <= this.Left) && (pPoint.Y >= this.Top) && (pPoint.Y - this.Height <= this.Top));
    }
    Postion(pTarget) {
        if (this.Right < pTarget.Left) {
            if (this.Top > pTarget.Bottom)
                return XDragType.RightTop;
            if (this.Bottom < pTarget.Top)
                return XDragType.RightBottom;
            return XDragType.Right;
        }
        if (pTarget.Right < this.Left) {
            if (pTarget.Bottom < this.Top)
                return XDragType.LeftTop;
            if (pTarget.Top > this.Bottom)
                return XDragType.LeftBottom;
            return XDragType.Left;
        }
        if (this.Top < pTarget.Bottom)
            return XDragType.Bottom;
        if (this.Bottom > pTarget.Top)
            return XDragType.Top;
        return XDragType.Error;
    }
}
XRect._Empty = new XRect(0, 0, 0, 0);
class XElement {
    static NextID() {
        return this._ID++;
    }
    constructor(pOwner, pClass = null, pTag = null) {
        this._IsVisible = true;
        this.UUID = 0;
        this.OnResize = null;
        this.OrderIndex = 0;
        this.Rows = 0;
        this.Cols = 0;
        this.Children = new XArray();
        this.AutoIncZIndex = false;
        this.UUID = XElement.NextID();
        this.Owner = pOwner;
        this.HTML = this.CreateContainer(pTag);
        this.HTML.Object = this;
        if (pClass == null)
            pClass = this.constructor.name;
        this.Element = null;
        this.CreateChildren();
        if (!X.IsEmpty(pClass))
            this.HTML.className = pClass;
        if (pOwner instanceof XElement)
            pOwner.HTML.appendChild(this.HTML);
        if (pOwner instanceof HTMLElement)
            pOwner.appendChild(this.HTML);
        this._ResizeObserver = new ResizeObserver(() => this.SizeChanged());
        this._ResizeObserver.observe(this.HTML);
        if (pOwner instanceof XElement)
            pOwner.AddChildren(this);
    }
    GetOwner(pPredicate) {
        var p = this.Owner;
        while (p != null) {
            if (pPredicate(p))
                return p;
            p = p.Owner;
        }
        return null;
    }
    GetDialogContainer() {
        return this.GetOwner(o => o.IsDialogContainer === true);
    }
    IncZIndex() {
        this.HTML.style.zIndex = XPopupManager.ZIndex();
    }
    AddChildren(pElement) {
        this.Children.Add(pElement);
    }
    get Rect() {
        return this.HTML.GetRect();
    }
    set Rect(pValue) {
        this.HTML.SetRect(pValue);
    }
    SizeChanged() {
        if (this.OnResize != null)
            this.OnResize.apply(this, [this]);
    }
    CheckClose(pElement) {
        return true;
    }
    get IsDrawed() {
        var elm = this.HTML;
        while (elm !== null && elm !== document.body) {
            if (elm.parentElement == document.body)
                return true;
            var style = window.getComputedStyle(elm);
            if (style.display == "none")
                return false;
            elm = elm.parentElement;
        }
        return false;
    }
    OnHide() {
    }
    OnShow() {
    }
    Show(pValue = true) {
        if (this.AutoIncZIndex)
            this.IncZIndex();
        var old = this.IsDrawed;
        this._IsVisible = pValue;
        if (pValue === true) {
            this.HTML.style.visibility = 'visible';
            this.OnShow();
        }
        else if (pValue === false) {
            this.HTML.style.visibility = 'hidden';
            this.OnHide();
        }
        if (pValue == old)
            return;
    }
    SetContent(pValue) {
        if (this.HTML != null)
            this.HTML.innerText = pValue;
    }
    CreateChildren() {
    }
    CreateContainer(pTag = null) {
        throw new Error("Method not implemented.");
    }
    CanClose(pSource) {
        return true;
    }
    get IsVisible() {
        if (!this._IsVisible)
            return this._IsVisible;
        return this.IsDrawed;
    }
    set IsVisible(pValue) {
        this.Show(pValue);
    }
}
XElement._ID = 0;
/// <reference path="Base/XElement.ts" />
class XDiv extends XElement {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
    }
    CreateContainer() {
        return XUtils.AddElement(null, "div", null);
    }
}
/// <reference path="../XDiv.ts" />
class XBaseInput extends XDiv {
    constructor(pOwner) {
        super(pOwner, "InputContainer");
        this.NewLine = false;
        this.OrderIndex = -1;
        this.Input = this.CreateInput();
        this.ELMTitle = new XDiv(this, "InputTitle");
    }
    get Title() {
        return this.ELMTitle.HTML.innerHTML;
    }
    set Title(pValue) {
        this.ELMTitle.HTML.innerHTML = pValue;
    }
    CreateInput() {
        return XUtils.AddElement(this.HTML, "input", "XBaseButtonInput");
    }
}
/// <reference path="../Elements/Base/XBaseInput.ts" />
class XDataGridEditor extends XBaseInput {
    constructor(pOwner) {
        super(pOwner);
        this.Title = "Grade de Dados";
        this.DataGrid = new XDataGrid(this, "XDataGridEditor");
        this.Input = this.DataGrid.HTML;
    }
    CreateInput() {
        return null;
    }
}
/// <reference path="XBaseInput.ts" />
class XBaseLoockupInput extends XBaseInput {
    constructor(pOwner) {
        super(pOwner);
        this.Button = new XBaseButton(this, "XLookupButton");
        this.DropDownContent = this.CreateDropDown();
        XEventManager.AddEvent(this, this.Button.HTML, XEventType.Click, this.OnClick, true);
    }
    CreateDropDown() {
        return new XDropDownElement(this.Owner, "XDropDown");
    }
    OnClick(pArg) {
        this.DropDownContent.BindTo(this);
        this.DropDownContent.Show();
    }
}
/// <reference path="XDiv.ts" />
class XDataGrid extends XDiv {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
        var data = [];
        for (let i = 0; i < 20; i++) {
            const row = {
                id: i,
                nome: `Nome ${i}`,
                email: `email${i}@exemplo.com`,
                cidade: `Cidade ${i % 100}`,
                idade: 20 + (i % 50),
                telefone: `(11) 9${Math.floor(1000 + Math.random() * 9000)}-${Math.floor(1000 + Math.random() * 9000)}`,
                empresa: `Empresa ${i % 20}`,
                cargo: `Cargo ${i % 10}`,
                salario: 2000 + (i % 50) * 100,
                dataAdmissao: `${(i % 28 + 1).toString().LPad(2, '0')}/01/2023`,
                status: i % 4 === 0 ? 'Ativo' : 'Inativo',
                cargo1: `Cargo ${i % 10}`,
                salario2: 2000 + (i % 50) * 100,
                dataAdmissao3: `${(i % 28 + 1).toString().LPad(2, '0')}/01/2023`,
                nome1: `Nome ${i}`,
                email1: `email${i}@exemplo.com`,
                cidade1: `Cidade ${i % 100}`,
                idade1: 20 + (i % 50),
                telefone1: `(11) 9${Math.floor(1000 + Math.random() * 9000)}-${Math.floor(1000 + Math.random() * 9000)}`,
                empresa1: `Empresa ${i % 20}`,
            };
            data.push(row);
        }
        this.Table = new XTable(this, "XTable");
        this.Table.SetDataSet(data);
    }
}
//class XDataGridx extends XElement
//{
//    data: any = [];
//    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
//    {
//        super(pOwner, pClass);
//        for (let i = 0; i < 100; i++)
//        {
//            const row: any = {
//                id: i,
//                nome: `Nome ${i}`,
//                email: `email${i}@exemplo.com`,
//                cidade: `Cidade ${i % 100}`,
//                idade: 20 + (i % 50),
//                telefone: `(11) 9${Math.floor(1000 + Math.random() * 9000)}-${Math.floor(1000 + Math.random() * 9000)}`,
//                empresa: `Empresa ${i % 20}`,
//                cargo: `Cargo ${i % 10}`,
//                salario: 2000 + (i % 50) * 100,
//                dataAdmissao: `${(i % 28 + 1).toString().LPad(2, '0')}/01/2023`,
//                status: i % 4 === 0 ? 'Ativo' : 'Inativo',
//                cargo1: `Cargo ${i % 10}`,
//                salario2: 2000 + (i % 50) * 100,
//                dataAdmissao3: `${(i % 28 + 1).toString().LPad(2, '0')}/01/2023`,
//                nome1: `Nome ${i}`,
//                email1: `email${i}@exemplo.com`,
//                cidade1: `Cidade ${i % 100}`,
//                idade1: 20 + (i % 50),
//                telefone1: `(11) 9${Math.floor(1000 + Math.random() * 9000)}-${Math.floor(1000 + Math.random() * 9000)}`,
//                empresa1: `Empresa ${i % 20}`,
//            };
//            this.DataSet.push(row);
//        }
//        this.Container = new XDiv(this, "XDataGrid");
//        this.container = this.Container.HTML;
//        const fields = Object.keys(this.DataSet[0] || {});
//        this.Columns = fields.map(field => ({ field, visible: true, width: 120 }));
//        this.render();
//        this.addColumnVisibilityToggle();
//    }
//    Container: XDiv;
//    Table: XTable | null = null;
//    private container: HTMLElement;
//    private DataSet: any[] = [];
//    private Columns: XColumnConfig[];
//    private _SortState: { field: string; direction: 'asc' | 'desc' } | null = null;
//    private rowNumberColumn: XColumnConfig = { Title: '#', Visible: true, Width: 50 };
//    protected override CreateContainer(): HTMLElement 
//    {
//        return XUtils.AddElement<HTMLElement>(null, "div", null);
//    }
//    private render()
//    {
//        this.container.innerHTML = '';
//        this.Table = new XTable(this.Container, "");
//        const table = this.Table;
//        //table.style.minWidth = `${this.columns.reduce((acc, col) => acc + (col.visible ? col.width : 0), this.rowNumberColumn.width)}px`;
//        this.buildHeader(table);
//        //this.buildBody(table);
//        //this.container.appendChild(table);
//    }
//    private buildHeader(table: XTable)
//    {
//        this.Columns.filter(c => c.Visible).forEach(colConfig =>
//        {
//            const th = this.createHeaderTh(colConfig);
//            headerRow.appendChild(th);
//        });
//    }
//    private createHeaderTh(colConfig: XColumnConfig): HTMLTableCellElement
//    {
//        const th = document.createElement('th');
//        th.textContent = colConfig.Title;
//        //th.style.width = `${colConfig.width}px`;
//        th.style.userSelect = 'none';
//        //if (colConfig.field !== '#')
//        th.draggable = colConfig.Title !== '#';
//        const sortIcon = document.createElement('span');
//        sortIcon.className = 'sort-icon';
//        if (this._SortState?.field === colConfig.Title)
//        {
//            sortIcon.textContent = this._SortState.direction === 'asc' ? ' ▲' : ' ▼';
//        }
//        th.appendChild(sortIcon);
//        // Drag para reordenar colunas
//        th.addEventListener('dragstart', (e) =>
//        {
//            e.dataTransfer?.setData('text/plain', colConfig.Title);
//            th.classList.add('dragging');
//        });
//        th.addEventListener('dragend', () =>
//        {
//            th.classList.remove('dragging');
//        });
//        th.addEventListener('dragover', (e) =>
//        {
//            e.preventDefault();
//            if (!th.classList.contains('drag-over'))
//            {
//                th.classList.add('drag-over');
//            }
//        });
//        th.addEventListener('dragleave', () =>
//        {
//            th.classList.remove('drag-over');
//        });
//        th.addEventListener('drop', (e) =>
//        {
//            e.preventDefault();
//            th.classList.remove('drag-over');
//            const draggedField = e.dataTransfer?.getData('text/plain');
//            const visibleColumns = this.getVisibleColumns();
//            const draggedIndex = visibleColumns.findIndex(c => c.Title === draggedField);
//            const targetIndex = visibleColumns.findIndex(c => c.Title === colConfig.Title);
//            if (draggedIndex === -1 || targetIndex === -1 || draggedIndex === targetIndex) return;
//            // Reordenar colunas originais mantendo a referência
//            const originalDraggedIndex = this.Columns.findIndex(c => c.Title === draggedField);
//            const originalTargetIndex = this.Columns.findIndex(c => c.Title === colConfig.Title);
//            [this.Columns[originalDraggedIndex], this.Columns[originalTargetIndex]] =
//                [this.Columns[originalTargetIndex], this.Columns[originalDraggedIndex]];
//            this.render();
//        });
//        // Redimensionador
//        const resizer = document.createElement('div');
//        resizer.className = 'resizer';
//        th.appendChild(resizer);
//        this.addResizerEvents(th, resizer, colConfig);
//        // Clique para ordenar
//        th.addEventListener('click', () => this.sortData(colConfig.Title));
//        return th;
//    }
//    private addResizerEvents(th: HTMLTableCellElement, resizer: HTMLDivElement, colConfig: XColumnConfig)
//    {
//        let isResizing = false;
//        let startX = 0;
//        let startWidth = 0;
//        resizer.addEventListener('mousedown', (e) =>
//        {
//            e.stopPropagation(); // Impede a propagação para o elemento pai
//            e.preventDefault(); // Evita comportamento padrão indesejado
//            isResizing = true;
//            startX = e.clientX;
//            startWidth = th.offsetWidth;
//            const handleMouseMove = (e: MouseEvent) =>
//            {
//                if (!isResizing) return;
//                const newWidth = startWidth + (e.clientX - startX);
//                th.style.width = `${newWidth}px`;
//                colConfig.Width = newWidth;
//                this.updateColumnWidths(colConfig.Title, newWidth);
//            };
//            const handleMouseUp = () =>
//            {
//                isResizing = false;
//                document.removeEventListener('mousemove', handleMouseMove);
//                document.removeEventListener('mouseup', handleMouseUp);
//            };
//            document.addEventListener('mousemove', handleMouseMove);
//            document.addEventListener('mouseup', handleMouseUp, { once: true });
//        });
//    }
//    private updateColumnWidths(field: string, width: number)
//    {
//        const index = this.Columns.findIndex(c => c.Title === field);
//        if (index > -1)
//        {
//            this.Columns[index].Width = width;
//            document.querySelectorAll(`th[data-field="${field}"], td[data-field="${field}"]`)
//                .forEach(el => (el as HTMLElement).style.width = `${width}px`);
//        }
//    }
//    private getVisibleColumns(): XColumnConfig[]
//    {
//        return [this.rowNumberColumn, ...this.Columns.filter(c => c.Visible)];
//    }
//    private sortData(field: string)
//    {
//        if (this._SortState?.field === field)
//        {
//            this._SortState.direction = this._SortState.direction === 'asc' ? 'desc' : 'asc';
//        } else
//        {
//            this._SortState = { field, direction: 'asc' };
//        }
//        var self = this._SortState;
//        this.DataSet.sort((a, b) =>
//        {
//            var e: any = this;
//            if (a[field] > b[field])
//                return e._SortState.direction === 'asc' ? 1 : -1;
//            if (a[field] < b[field])
//                return e._SortState.direction === 'asc' ? -1 : 1;
//            return 0;
//        });
//        this.render();
//    }
//    private addColumnVisibilityToggle()
//    {
//        const button = document.createElement('button');
//        button.className = 'menu-button';
//        button.textContent = '☰';
//        const menu = document.createElement('div');
//        menu.className = 'column-menu';
//        this.Columns.forEach(col =>
//        {
//            const label = document.createElement('label');
//            const checkbox = document.createElement('input');
//            checkbox.type = 'checkbox';
//            checkbox.checked = col.Visible;
//            checkbox.addEventListener('change', () =>
//            {
//                col.Visible = checkbox.checked;
//                this.render();
//            });
//            label.appendChild(checkbox);
//            label.appendChild(document.createTextNode(col.Title));
//            menu.appendChild(label);
//        });
//        button.addEventListener('click', () =>
//        {
//            menu.style.display = menu.style.display === 'block' ? 'none' : 'block';
//        });
//        this.HTML.appendChild(button);
//        this.HTML.appendChild(menu);
//    }
//    private buildBody(table: HTMLTableElement)
//    {
//        const tbody = document.createElement('tbody');
//        this.DataSet.forEach((rowData, index) =>
//        {
//            const tr = document.createElement('tr');
//            // Número sequencial
//            const tdNumber = document.createElement('td');
//            tdNumber.textContent = (index + 1).toString();
//            tr.appendChild(tdNumber);
//            // Dados
//            this.Columns.filter(c => c.Visible).forEach(colConfig =>
//            {
//                const td = document.createElement('td');
//                td.dataset.field = colConfig.Title;
//                td.style.width = `${colConfig.Width}px`;
//                tr.appendChild(td);
//                const txt = document.createElement('span');
//                txt.innerText = rowData[colConfig.Title];
//                td.appendChild(txt);
//            });
//            tbody.appendChild(tr);
//        });
//        table.appendChild(tbody);
//    }
//}
/// <reference path="../Elements/Base/XBaseLoockupInput.ts" />
/// <reference path="../Elements/XDataGrid.ts" />
class XDropDownDataGrid extends XDataGrid {
    constructor(pOwner, pClass) {
        super(pOwner.DropDownContent, pClass);
        this.Editor = pOwner;
    }
}
class XDataLoockupEditor extends XBaseLoockupInput {
    constructor(pOwner) {
        super(pOwner);
        this.Input.className = "XDataLoockupEditor";
        this.Title = "Digite uma Data";
        this.DataGrid = new XDropDownDataGrid(this, "XDropDownGrid");
        this.DataGrid.Table.OnRowClick = (rows) => this.OnSelected(rows);
    }
    OnSelected(pRows) {
        this.Input.value = pRows[0].Tupla.nome;
        this.DropDownContent.Selected();
    }
}
/// <reference path="XBaseInput.ts" />
class XBaseButtonInput extends XBaseInput {
    constructor(pOwner) {
        super(pOwner);
        this.Button = new XBaseButton(this, "XLookupButton");
        XEventManager.AddEvent(this, this.Button.HTML, XEventType.Click, this.OnClick, true);
    }
    OnClick(pArg) {
    }
}
/// <reference path="../Elements/Base/XBaseButtonInput.ts" />
class XDatePickerEditor extends XBaseLoockupInput {
    constructor(pOwner) {
        super(pOwner);
        this.SelectedDate = new Date();
        this.Calendar = this.DropDownContent;
        this.Input.className = "XDatePickerEditor";
        this.Calendar.IsVisible = false;
        this.Calendar.OnSelectdate = (d) => this.Selected(d);
        this.Calendar.ReferenceElement = this;
        this.Input.placeholder = 'dd/mm/aaaa';
        XEventManager.AddEvent(this, this.Input, XEventType.Input, this.HandleInput);
        this.Title = "Digite uma Data";
    }
    CreateDropDown() {
        this.Calendar = new XCalendar(this.Owner, "XCalendar");
        return this.Calendar;
    }
    Selected(pDate) {
        this.SelectedDate = pDate;
        this.Calendar.IsVisible = false;
        this.Input.value = this.FormatDate(pDate, this.Input.placeholder);
    }
    FormatDate(data, formato) {
        const completarComZero = (valor) => {
            return valor < 10 ? `0${valor}` : valor.toString();
        };
        // Extrai componentes da data
        const dia = completarComZero(data.getDate());
        const mes = completarComZero(data.getMonth() + 1); // +1 pois meses são 0-based
        const ano = data.getFullYear().toString();
        const horas = completarComZero(data.getHours());
        const minutos = completarComZero(data.getMinutes());
        const segundos = completarComZero(data.getSeconds());
        // Detecta partes do formato
        const partes = formato.split(' ');
        const formatoData = partes.find(p => p.toLowerCase().includes('dd')) || null;
        const formatoHora = partes.find(p => p.toLowerCase().includes('hh')) || null;
        // Monta a string
        let resultado = '';
        // Formata a data se necessário
        if (formatoData)
            resultado = `${dia}/${mes}/${ano}`;
        // Formata a hora se necessário
        if (formatoHora) {
            const separador = resultado ? ' ' : ''; // Espaço se já tiver data
            let horaFormatada = `${horas}:${minutos}`;
            // Adiciona segundos se necessário
            if (formatoHora.toLowerCase().includes('ss'))
                horaFormatada += `:${segundos}`;
            resultado += separador + horaFormatada;
        }
        return resultado;
    }
    HandleInput(pEvent) {
        this.ValidateDate();
        const input = pEvent.target;
        const value = input.value.replace(/\D/g, '');
        const placeholder = this.Input.placeholder;
        const [datePart, timePart] = placeholder.includes(' ') ?
            placeholder.split(' ') : [placeholder.startsWith('dd')
                ? placeholder : null, placeholder.startsWith('hh') ? placeholder : null];
        let formatted = '';
        let remainingDigits = value;
        if (datePart === null || datePart === void 0 ? void 0 : datePart.startsWith('dd/mm/aaaa')) {
            const dateDigits = remainingDigits.slice(0, 8);
            formatted = this.formatDateSection(dateDigits);
            remainingDigits = remainingDigits.slice(8);
        }
        if (timePart === null || timePart === void 0 ? void 0 : timePart.startsWith('hh')) {
            if (formatted !== '' && X.Length(formatted) == X.Length(datePart))
                formatted += ' ';
            formatted += this.formatTimeSection(remainingDigits, timePart === 'hh:MM:ss' ? 6 : 4);
        }
        if (pEvent.inputType == "deleteContentBackward")
            formatted = formatted.trim();
        input.value = formatted;
    }
    formatDateSection(pDigits) {
        let formatted = '';
        for (let i = 0; i < pDigits.length; i++) {
            if (i === 2 || i === 4)
                formatted += '/';
            formatted += pDigits[i];
        }
        return formatted;
    }
    formatTimeSection(pDigits, pMax) {
        let formatted = '';
        for (let i = 0; i < Math.min(pDigits.length, pMax); i++) {
            if (i === 2 || i === 4)
                formatted += ':';
            formatted += pDigits[i];
        }
        return formatted;
    }
    ValidateDate() {
        this.Input.classList.remove('Error');
        if (X.IsEmpty(this.Input.value))
            return;
        if (!Date.IsDateOrTime(this.Input.value))
            this.Input.classList.add('Error');
        else {
            const [d, m, y] = this.Input.value.split('/');
            const date = new Date(`${y}-${m}-${d}`);
            if (this.Calendar.IsVisible)
                this.Calendar.SelectDate(date);
        }
    }
    OnClick(pArg) {
        this.Calendar.BindTo(this);
        this.Calendar.Show();
        this.Calendar.SelectedDate = this.SelectedDate;
    }
}
/// <reference path="../Elements/Base/XBaseInput.ts" />
class XDecimalEditor extends XBaseInput {
    constructor(pOwner) {
        super(pOwner);
        this.Input.className = "XDecimalEditor";
        this.AllowNegative = false;
        this.MaxIntegerDigits = 4;
        this.DecimalDigits = 2;
        XEventManager.AddEvent(this, this.Input, XEventType.Input, this.HandleInput);
        XEventManager.AddEvent(this, this.Input, XEventType.KeyDown, this.HandleKeydown);
        this.Input.value = this.FormatValue(this.ProcessValue(''));
        this.Title = "Digite um Valor Decimal";
    }
    HandleInput(event) {
        const position = this.Input.selectionStart;
        const value = this.Input.value;
        const processed = this.ProcessValue(value);
        const formatted = this.FormatValue(processed);
        if (this.Input.value !== formatted) {
            this.Input.value = formatted;
            this.AdjustCursorPosition(position, value, formatted);
        }
    }
    HandleKeydown(pArg) {
        if (pArg.key === '-') {
            pArg.preventDefault();
            if (this.AllowNegative) {
                const processed = this.ProcessValue(this.Input.value);
                processed.isNegative = !processed.isNegative;
                this.Input.value = this.FormatValue(processed);
            }
        }
        else if (pArg.ctrlKey && (pArg.key === 'Home' || pArg.key === 'End')) {
            pArg.preventDefault();
            this.Input.setSelectionRange(0, this.Input.value.length);
        }
        else if (pArg.key === ',' && this.Input.value.includes(','))
            pArg.preventDefault();
    }
    ProcessValue(value) {
        let rawValue = value.replace(/[^\d-,]/g, '');
        let isNegative = false;
        if (this.AllowNegative) {
            isNegative = rawValue.startsWith('-');
            rawValue = rawValue.replace(/-/g, '');
        }
        const [integer = '0', decimal = ''] = rawValue.split(',');
        const integerClean = integer.replace(/\D/g, '').replace(/^0+/, '').substring(0, this.MaxIntegerDigits) || '0';
        const decimalClean = decimal.replace(/\D/g, '').slice(-this.DecimalDigits).RPad(this.DecimalDigits, '0');
        return {
            isNegative: isNegative && this.AllowNegative,
            integerPart: integerClean || '0',
            decimalPart: decimalClean
        };
    }
    FormatValue(processed) {
        const formattedInteger = processed.integerPart
            .split('')
            .reverse()
            .join('')
            .replace(/(\d{3})(?=\d)/g, '$1.')
            .split('')
            .reverse()
            .join('')
            .replace(/^\./, '') || '0';
        const sign = processed.isNegative ? '-' : '';
        return `${sign}${formattedInteger},${processed.decimalPart}`;
    }
    AdjustCursorPosition(oldPos, oldValue, newValue) {
        if (oldPos === null)
            return;
        const commaIndex = newValue.indexOf(',');
        const isDecimal = oldPos > oldValue.indexOf(',');
        if (isDecimal && commaIndex !== -1) {
            const decimalCursor = Math.min(oldPos - oldValue.indexOf(',') - 1 + commaIndex + 1, newValue.length);
            this.Input.setSelectionRange(decimalCursor, decimalCursor);
        }
        else
            this.Input.setSelectionRange(oldPos, oldPos);
    }
}
/// <reference path="../Elements/Base/XBaseInput.ts" />
class XEMailEditor extends XBaseInput {
    constructor(pOwner) {
        super(pOwner);
        this.Input.className = "XEMailEditor";
        this.Input.placeholder = "e-mail";
        XEventManager.AddEvent(this, this.HTML, XEventType.Input, this.Validate);
        this.Title = "Digite um E-Mail";
    }
    Validate(pArg) {
        pArg.preventDefault(); // Impede o envio do formulário
        const email = this.Input.value;
        var isvalid = email.match(/^[^\s@]+@[^\s@]+\.[^\s@]+$/);
        this.Input.classList.remove('Error');
        if (isvalid)
            return;
        if (!isvalid)
            this.Input.classList.add('Error');
    }
}
/// <reference path="../Elements/Base/XBaseInput.ts" />
class XIntegerEditor extends XBaseInput {
    constructor(pOwner) {
        super(pOwner);
        this.Mask = "#.##0";
        this.AllowNegative = false;
        this.Input.className = "XIntegerEditor";
        this.AllowNegative = this.Mask.startsWith('-');
        this.MaxDigits = this.Mask.replace(/[^#0]/g, '').length;
        this.HasSeparator = this.Mask.includes('.');
        this.IsFixedMask = !this.Mask.includes('#') && /^[-]?0+$/.test(this.Mask);
        this.Init();
        this.Title = "Digite um Valor Inteiro";
    }
    Init() {
        this.Input.value = this.FormatNumber('0');
        this.SetupEventListeners();
    }
    SetupEventListeners() {
        XEventManager.AddEvent(this, this.Input, XEventType.Input, this.GandleInput);
        XEventManager.AddEvent(this, this.Input, XEventType.KeyDown, this.handleKeyDown);
        XEventManager.AddEvent(this, this.Input, XEventType.Blur, this.HandleBlur);
    }
    GandleInput(pArg) {
        const rawValue = this.GetRawValue(this.Input.value);
        const processed = this.ProcessValue(rawValue);
        this.Input.value = this.FormatNumber(processed);
    }
    handleKeyDown(e) {
        if ([8, 46, 9, 27, 13, 37, 38, 39, 40, 36, 35].Contains(e.keyCode))
            return;
        if ((e.ctrlKey || e.metaKey) && [67, 86, 88, 65].Contains(e.keyCode))
            return;
        const isNegativeSign = e.key === '-' && this.AllowNegative;
        const isNumber = e.keyCode >= 48 && e.keyCode <= 57 || e.keyCode >= 96 && e.keyCode <= 105;
        if (isNegativeSign) {
            e.preventDefault();
            this.ToggleSign();
            return;
        }
        if (!isNumber) {
            e.preventDefault();
        }
    }
    ToggleSign() {
        const current = this.GetRawValue(this.Input.value);
        const newValue = current.startsWith('-') ? current.slice(1) : `-${current}`;
        this.Input.value = this.FormatNumber(newValue);
    }
    HandleBlur() {
        const rawValue = this.GetRawValue(this.Input.value);
        if (rawValue === '-' || rawValue === '') {
            this.Input.value = '0';
        }
        else {
            this.Input.value = this.FormatNumber(rawValue);
        }
    }
    GetRawValue(pValue) {
        return pValue.replace(/[^0-9-]/g, '');
    }
    ProcessValue(pValue) {
        let isNegative = this.AllowNegative && pValue.startsWith('-');
        let digits = pValue.replace(/-/g, '').replace(/^0+/, '') || '0';
        digits = digits.slice(0, this.MaxDigits);
        if (this.IsFixedMask) {
            digits = digits.RPad(this.MaxDigits, '0');
        }
        return (isNegative ? '-' : '') + digits;
    }
    FormatNumber(pValue) {
        const isNegative = pValue.startsWith('-');
        let digits = isNegative ? pValue.slice(1) : pValue;
        digits = digits.replace(/[^0-9]/g, '');
        if (this.HasSeparator) {
            digits = this.InsertSeparators(digits);
        }
        if (this.IsFixedMask) {
            digits = digits.LPad(this.MaxDigits, '0');
        }
        return (isNegative ? '-' : '') + digits;
    }
    InsertSeparators(pDigits) {
        var _a, _b, _c, _d;
        return (_d = (_c = (_b = (_a = pDigits === null || pDigits === void 0 ? void 0 : pDigits.split('')) === null || _a === void 0 ? void 0 : _a.reverse()) === null || _b === void 0 ? void 0 : _b.join('')) === null || _c === void 0 ? void 0 : _c.match(/.{1,3}/g)) === null || _d === void 0 ? void 0 : _d.join('.').split('').reverse().join('').replace(/^\./, '');
    }
}
/// <reference path="../Elements/Base/XBaseInput.ts" />
class XMemoEditor extends XBaseInput {
    constructor(pOwner) {
        super(pOwner);
        this.Input.className = "XMemoEditor";
        this.Title = "Digite um Texto";
    }
    CreateInput() {
        return XUtils.AddElement(this.HTML, "textarea", "XBaseButtonInput");
    }
}
/// <reference path="../Elements/Base/XBaseInput.ts" />
class XNormalEditor extends XBaseInput {
    constructor(pOwner) {
        super(pOwner);
        this.Input.className = "XNormalEditor";
        this.Title = "Digite uma Frase";
    }
}
/// <reference path="../Elements/Base/XBaseInput.ts" />
class PhoneFormatter {
    static format(value) {
        let nums = value.replace(/\D/g, '');
        let formatted = '';
        // Limitar tamanho máximo
        const maxLength = nums.startsWith('55') ? 13 : 11;
        nums = nums.substring(0, maxLength);
        if (nums.startsWith('55') && nums.length > 2) {
            formatted = `+55 `;
            nums = nums.substring(2);
        }
        if (nums.length >= 2) {
            formatted += `(${nums.substring(0, 2)})`;
            nums = nums.substring(2);
        }
        if (nums.length > 0) {
            formatted += ' ' + nums.replace(/(\d{4,5})(\d{4})$/, '$1-$2');
        }
        return formatted.replace(/(\s)-/g, '$1').trim();
    }
    static validate(phone) {
        return /^(?:\+55\s\(\d{2}\)\s\d{5}-\d{4}$|\(\d{2}\)\s\d{4,5}-\d{4}$)/.test(phone);
    }
}
class XPhoneEditor extends XBaseInput {
    constructor(pOwner) {
        super(pOwner);
        this.lastValue = '';
        this.cursorPos = 0;
        this.Input.className = "XPhoneEditor";
        this.Title = "Digite um Nº de Telefone";
        XEventManager.AddEvent(this, this.Input, XEventType.Input, this.handleInput);
        XEventManager.AddEvent(this, this.Input, XEventType.KeyDown, this.handleKeyDown);
    }
    handleKeyDown(e) {
        // Permitir navegação e comandos especiais
        const allowedKeys = ['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Home', 'End', 'Tab', 'Control'];
        if (allowedKeys.Contains(e.key) || e.ctrlKey)
            return;
        if (!/\d/.test(e.key))
            e.preventDefault();
    }
    handleInput() {
        const prevValue = this.Input.value;
        const rawValue = prevValue.replace(/\D/g, '');
        // Controlar máximo de dígitos
        const maxLength = rawValue.startsWith('55') ? 13 : 11;
        const newValue = PhoneFormatter.format(rawValue.substring(0, maxLength));
        // Manter posição do cursor
        const cursorBefore = this.Input.selectionStart || 0;
        const diff = newValue.length - prevValue.length;
        // Ajustar posição do cursor
        let newCursorPos = cursorBefore;
        if (this.lastValue.length > newValue.length) {
            newCursorPos = this.calculateCursorPos(cursorBefore, prevValue, newValue);
        }
        else {
            newCursorPos = cursorBefore + diff;
        }
        this.Input.value = newValue;
        this.lastValue = newValue;
        // Corrigir posição do cursor
        requestAnimationFrame(() => {
            this.Input.setSelectionRange(newCursorPos, newCursorPos);
        });
        this.updateValidation();
    }
    calculateCursorPos(oldPos, oldValue, newValue) {
        let adjust = 0;
        const isBackspace = oldValue.length > newValue.length;
        // Ajustar para formatação automática
        for (let i = 0; i < oldPos; i++) {
            if (isBackspace && /[()\-\s]/.test(oldValue[i])) {
                adjust--;
            }
            if (!isBackspace && /[()\-\s]/.test(newValue[i])) {
                adjust++;
            }
        }
        return Math.max(0, Math.min(newValue.length, oldPos + adjust));
    }
    updateValidation() {
        const isvalid = PhoneFormatter.validate(this.Input.value);
        this.Input.classList.remove('Error');
        if (isvalid)
            return;
        if (!isvalid)
            this.Input.classList.add('Error');
    }
}
class XSizeableElement extends XDiv {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
        this._Location = XPoint.Empty;
        this._Start = XPoint.Empty;
        this._StartPos = XPoint.Empty;
        this.DragType = XDragType.Error;
        this.IsCaptured = false;
        this.AutoPosition = true;
        this.CustomSizePosition = false;
        this.ParentConstraint = true;
        this.DragRect = XRect.Empty;
        this._CanDrag = true;
        this._CanResize = true;
        this.PrepareEvents();
    }
    get CanDrag() { return this._CanDrag; }
    get CanResize() { return this._CanResize; }
    set CanDrag(pValue) { this._CanDrag = pValue; }
    set CanResize(pValue) { this._CanResize = pValue; }
    PrepareEvents() {
        if (!this.CanDrag && !this.CanResize)
            return;
        XEventManager.AddEvent(this, this.HTML, XEventType.MouseDown, this.OnMouseDown, true);
        XEventManager.AddEvent(this, this.HTML, XEventType.MouseMove, this.OnMouseMove);
        XEventManager.AddEvent(this, this.HTML, XEventType.MouseLeave, this.OnMouseLeave, true);
        XEventManager.AddEvent(this, this.HTML, XEventType.MouseUp, this.OnMouseUp, true);
    }
    SelectionChanged() {
        //if (this.IsSelected)
        //    this.HTML.setAttribute("IsSelected", "true");
        //else
        //    this.HTML.removeAttribute("IsSelected");
    }
    get IsDraging() {
        return XDragUtils.HasDrag == this;
    }
    Focus() {
    }
    Resize() {
        if (this.AutoPosition)
            this.DoAutoPosition();
        if (this.DragPanelSizeEvent != null)
            this.DragPanelSizeEvent(this);
        this.Focus();
    }
    DoAutoPosition() {
        if (this.HTML.parentElement == null)
            return;
        if (this.HTML.clientWidth > 0 && this.HTML.clientHeight > 0) {
            this.HTML.style.left = ((this.HTML.parentElement.clientWidth - this.HTML.clientWidth) / 2) + "px";
            this.HTML.style.top = ((this.HTML.parentElement.clientHeight - this.HTML.clientHeight) / 2) + "px";
        }
    }
    GetLineCount(pRect) {
        var cnt = new XArray([0, 0, 0, 0, 0]);
        if (cnt[0] > 0)
            cnt[0] = pRect.Height / (cnt[0] + 1);
        if (cnt[1] > 0)
            cnt[1] = pRect.Width / (cnt[1] + 1);
        if (cnt[2] > 0)
            cnt[2] = pRect.Height / (cnt[2] + 1);
        if (cnt[3] > 0)
            cnt[3] = pRect.Width / (cnt[3] + 1);
        return cnt;
    }
    get DragHeight() {
        return this.DragRect.Height;
    }
    get ResizeWidth() {
        return 4;
    }
    OnMouseLeave(pArg) {
        if (!XDragUtils.HasDrag && this.HTML.parentElement != null) {
            pArg.stopPropagation();
            this.HTML.parentElement.style.cursor = "unset";
        }
    }
    OnMouseDown(pArg) {
        this.StartMouseDown(pArg);
        if (this.AutoIncZIndex)
            this.IncZIndex();
        if (!pArg.ctrlKey && pArg.button == 0) {
            XPopupManager.HideAll(pArg);
            var rect = this.HTML.GetRect();
            this._Start = new XPoint(pArg.pageX - rect.Left, pArg.pageY - rect.Top);
            this._StartPos = new XPoint(pArg.pageX, pArg.pageY);
            this.DragType = this.CanExecute(this.GetLocationType(this._Start, this.HTML.offsetWidth, this.HTML.offsetHeight, this.ResizeWidth, this.DragHeight));
            if (this.DragType != XDragType.Error) {
                pArg.stopPropagation();
                XEventManager.AddEvent(this, window, XEventType.MouseUp, this.OnMouseUp);
                XDragUtils.HasDrag = this;
                XEventManager.AddEvent(this, document.body, XEventType.MouseMove, this.DragMouseMove);
                XDragUtils.HasDrag.IsCaptured = true;
                this._Location = XUtils.Location(this.HTML);
            }
        }
    }
    StartMouseDown(pArg) {
    }
    GetLocationType(pStart, pClientWidth, pClientHeight, pResizeWidth, pDragHeight) {
        let dt = pStart.LocationType(pClientWidth, pClientHeight, pResizeWidth, pDragHeight);
        if (dt == XDragType.Drag && this.DragRect != null && !this.DragRect.Contains(pStart)) {
            this.DragRect.Contains(pStart);
            return XDragType.Error;
        }
        return dt;
    }
    GetPos(pSource, pTarget) {
        return pSource.Postion(pTarget);
    }
    OnMouseUp(pArg) {
        this.StartMouseDown(pArg);
        if (XDragUtils.HasDrag != null && XDragUtils.HasDrag == this && this.HTML.parentElement != null) {
            pArg.stopPropagation();
            XEventManager.RemoveEvent(XDragUtils.HasDrag, window, XEventType.MouseUp);
            XEventManager.RemoveEvent(XDragUtils.HasDrag, document.body, XEventType.MouseMove);
            XDragUtils.HasDrag.IsCaptured = false;
            var oldtype = XDragUtils.HasDrag.DragType;
            this.HTML.parentElement.style.cursor = "unset";
            XDragUtils.HasDrag = null;
            if (oldtype == XDragType.Drag)
                this.EndDrag();
            else if (oldtype != XDragType.Error)
                this.EndSize();
        }
    }
    EndDrag() { }
    EndSize() {
    }
    OnMouseMove(pArg) {
        if (this.HTML.parentElement == null)
            return;
        if (pArg.target == this.HTML && XDragUtils.HasDrag == null && !pArg.ctrlKey) {
            pArg.stopPropagation();
            var rect = this.HTML.GetRect();
            var pt = new XPoint(pArg.pageX - rect.Left, pArg.pageY - rect.Top);
            this.DragType = this.CanExecute(this.GetLocationType(pt, this.HTML.offsetWidth, this.HTML.offsetHeight, this.ResizeWidth, this.DragHeight));
            XUtils.SetCursor(this.HTML.parentElement, this.DragType);
            if (this.IsCaptured)
                this.DragMouseMove(pArg);
        }
        else
            XUtils.SetCursor(this.HTML.parentElement, XDragType.Error);
    }
    CanExecute(pType) {
        if (!this.CanDrag && pType == XDragType.Drag)
            return XDragType.Error;
        if (!this.CanResize && pType != XDragType.Drag)
            return XDragType.Error;
        return pType;
    }
    DragMouseMove(pArg) {
        if (this.HTML.parentElement == null)
            return;
        pArg.stopPropagation();
        var x = this.HTML.parentElement.scrollLeft;
        var y = this.HTML.parentElement.scrollTop;
        var mx = pArg.pageX - this._StartPos.X;
        var my = pArg.pageY - this._StartPos.Y;
        this._StartPos = new XPoint(pArg.pageX, pArg.pageY);
        switch (this.DragType) {
            case XDragType.LeftTop:
                this._Location = new XPoint(this._Location.X + mx, this._Location.Y + my);
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth - mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.left = x + this._Location.X + "px";
                this.HTML.style.top = y + this._Location.Y + "px";
                this.HTML.style.width = (this.HTML.clientWidth - mx) + "px";
                this.HTML.style.height = (this.HTML.clientHeight - my) + "px";
                break;
            case XDragType.Top:
                this._Location = new XPoint(this._Location.X, this._Location.Y + my);
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth - mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.top = y + this._Location.Y + "px";
                this.HTML.style.height = (this.HTML.clientHeight - my) + "px";
                break;
            case XDragType.RightTop:
                this._Location = new XPoint(this._Location.X + mx, this._Location.Y + my);
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth + mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.top = y + this._Location.Y + "px";
                this.HTML.style.width = (this.HTML.clientWidth + mx) + "px";
                this.HTML.style.height = (this.HTML.clientHeight - my) + "px";
                break;
            case XDragType.Right:
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth + mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.width = (this.HTML.clientWidth + mx) + "px";
                break;
            case XDragType.RightBottom:
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth + mx), (this.HTML.clientHeight + my)))
                    break;
                this.HTML.style.width = (this.HTML.clientWidth + mx) + "px";
                this.HTML.style.height = (this.HTML.clientHeight + my) + "px";
                break;
            case XDragType.Bottom:
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth + mx), (this.HTML.clientHeight + my)))
                    break;
                this.HTML.style.height = (this.HTML.clientHeight + my) + "px";
                break;
            case XDragType.LeftBottom:
                this._Location = new XPoint(this._Location.X + mx, this._Location.Y + my);
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth - mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.left = x + this._Location.X + "px";
                this.HTML.style.width = (this.HTML.clientWidth - mx) + "px";
                this.HTML.style.height = (this.HTML.clientHeight + my) + "px";
                break;
            case XDragType.Left:
                this._Location = new XPoint(this._Location.X + mx, this._Location.Y);
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth - mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.left = x + this._Location.X + "px";
                this.HTML.style.width = (this.HTML.clientWidth - mx) + "px";
                break;
            case XDragType.Drag:
                this._Location = new XPoint(this._Location.X + mx, this._Location.Y + my);
                this.HTML.style.left = x + this._Location.X + "px";
                this.HTML.style.top = y + this._Location.Y + "px";
                break;
        }
        this.CustomSizePosition = true;
        this.Draging();
    }
    Draging() { }
}
/// <reference path="XSizeableElement.ts" />
class XPopupElement extends XSizeableElement {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
        this.AutoClose = false;
        this.OnPopupClosed = null;
        this.ReferenceElement = null;
        this.ReferenceElement = this;
        this.HTML.style.visibility = 'hidden';
        this.HTML.style.zIndex = XPopupManager.ZIndex();
        XPopupManager.Add(this);
    }
    CallPopupClosed() {
    }
    Show(pValue = true) {
        var _a;
        if (this._DialogContainer == null) {
            this._DialogContainer = this.GetDialogContainer();
            if (this._DialogContainer.HTML != this.HTML) {
                (_a = this.HTML.parentElement) === null || _a === void 0 ? void 0 : _a.removeChild(this.HTML);
                this._DialogContainer.HTML.appendChild(this.HTML);
            }
        }
        super.Show(pValue);
    }
    CanClose(pElement) {
        if (this.ReferenceElement != null)
            return !pElement.IsChildOf(this.ReferenceElement.HTML, true) && this.CheckClose(pElement) && this.IsVisible && !pElement.IsChildOf(this.HTML, true);
        return true;
    }
}
class XDropDownElement extends XPopupElement {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
        this.IsVisible = false;
    }
    Selected() {
        XPopupManager.HideAll();
    }
    BindTo(pElement) {
        const editorRect = pElement.HTML.getBoundingClientRect();
        const dropdownRect = this.HTML.getBoundingClientRect();
        const viewportWidth = window.innerWidth;
        const viewportHeight = window.innerHeight;
        let top;
        const spaceBelow = viewportHeight - editorRect.bottom;
        const spaceAbove = editorRect.top;
        if (dropdownRect.height <= spaceBelow)
            top = editorRect.bottom;
        else if (dropdownRect.height <= spaceAbove)
            top = editorRect.top - dropdownRect.height;
        else
            top = spaceBelow > spaceAbove ? editorRect.bottom : editorRect.top - dropdownRect.height;
        let left;
        const spaceRight = viewportWidth - editorRect.left;
        if (dropdownRect.width <= spaceRight)
            left = editorRect.left;
        else {
            const spaceLeft = editorRect.right;
            if (dropdownRect.width <= spaceLeft)
                left = editorRect.right - dropdownRect.width;
            else
                left = Math.max(0, viewportWidth - dropdownRect.width);
        }
        this.HTML.style.position = 'fixed';
        this.HTML.style.top = `${top}px`;
        this.HTML.style.left = `${left}px`;
    }
}
/// <reference path="Base/XPopupElement.ts" />
/// <reference path="Base/XDropDownElement.ts" />
class XCalendar extends XDropDownElement {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
        this.CurrentPanel = 'days';
        this.OnSelectdate = null;
        this.Header = new XDiv(this.HTML, "XCalendar-Header");
        this.LeftArrow = new XBaseButton(this.Header, "XCalendarLeftArrow");
        this.CenterButton = new XBaseButton(this.Header, "XCalendarCenterButton");
        this.RightArrow = new XBaseButton(this.Header, "XCalendarRightArrow");
        this.DaysGrid = new XDiv(this, "XDaysGrid");
        this.MonthsGrid = new XDiv(this, "XMonthsGrid");
        this.MonthsGrid.IsVisible = false;
        this.YearsGrid = new XDiv(this, "XYearsGrid");
        this.YearsGrid.IsVisible = false;
        this.ViewDate = new Date();
        this.SelectedDate = new Date();
        this.UpdateCalendar();
        this.CenterButton.HTML.addEventListener('click', () => {
            this.CurrentPanel = this.CurrentPanel === 'days' ? 'months' : 'years';
            this.UpdateCalendar();
        });
        this.LeftArrow.HTML.addEventListener('click', () => this.Navigate(-1));
        this.RightArrow.HTML.addEventListener('click', () => this.Navigate(1));
    }
    get CanDrag() { return false; }
    get CanResize() { return false; }
    OnShow(pValue = true) {
        this.CurrentPanel = 'days';
        this.UpdateCalendar();
    }
    OnHide() {
        if (this.DaysGrid == null)
            return;
        this.DaysGrid.IsVisible = false;
        this.MonthsGrid.IsVisible = false;
        this.YearsGrid.IsVisible = false;
    }
    CallPopupClosed() {
        this.DaysGrid.IsVisible = false;
        this.MonthsGrid.IsVisible = false;
        this.YearsGrid.IsVisible = false;
    }
    ShowYears() {
        this.YearsGrid.IsVisible = true;
        this.YearsGrid.HTML.innerHTML = "";
        const currentYear = this.ViewDate.getFullYear();
        const decadeStart = currentYear - ((currentYear - 1) % 10) - 1;
        const decadeEnd = decadeStart + 10;
        const gridStartYear = decadeStart - (decadeStart % 16);
        for (let year = gridStartYear; year < gridStartYear + 16; year++) {
            const cell = document.createElement('div');
            cell.className = 'YearCell';
            cell.textContent = year.toString();
            const isCurrentDecade = year >= (decadeStart + 1) && year <= decadeEnd;
            if (!isCurrentDecade)
                cell.classList.add('Faded');
            if (year === new Date().getFullYear())
                cell.classList.add('Current');
            cell.addEventListener('click', () => {
                this.ViewDate.setFullYear(year);
                this.CurrentPanel = 'months';
                this.UpdateCalendar();
            });
            this.YearsGrid.HTML.appendChild(cell);
        }
    }
    ShowMonths() {
        this.MonthsGrid.IsVisible = true;
        this.MonthsGrid.HTML.innerHTML = "";
        for (let month = 0; month < 12; month++) {
            const cell = document.createElement('div');
            cell.className = 'MonthCell';
            cell.textContent = new Date(this.ViewDate.getFullYear(), month).toLocaleDateString('pt-BR', { month: 'long' });
            if (month === new Date().getMonth() && this.ViewDate.getFullYear() === new Date().getFullYear())
                cell.classList.add('Current');
            cell.addEventListener('click', () => {
                this.ViewDate.setMonth(month);
                this.CurrentPanel = 'days';
                this.UpdateCalendar();
            });
            this.MonthsGrid.HTML.appendChild(cell);
        }
    }
    ShowDays() {
        this.DaysGrid.IsVisible = true;
        this.DaysGrid.HTML.innerHTML = '';
        ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'].forEach((day, i) => {
            const cell = document.createElement('div');
            cell.textContent = day;
            cell.className = `Day-Header ${i === 0 ? 'Sunday' : ''} ${i === 6 ? 'Saturday' : ''}`;
            this.DaysGrid.HTML.appendChild(cell);
        });
        const firstDay = new Date(this.ViewDate.getFullYear(), this.ViewDate.getMonth(), 1);
        const lastDay = new Date(this.ViewDate.getFullYear(), this.ViewDate.getMonth() + 1, 0);
        let date = new Date(firstDay);
        date.setDate(date.getDate() - firstDay.getDay());
        for (let i = 0; i < 42; i++) {
            const cell = document.createElement('div');
            cell.className = 'DayCell';
            const isCurrentMonth = date.getMonth() === this.ViewDate.getMonth();
            const isToday = date.toDateString() === new Date().toDateString();
            const isSelected = this.SelectedDate && date.toDateString() === this.SelectedDate.toDateString();
            if (!isCurrentMonth)
                cell.classList.add('Faded');
            if (isToday)
                cell.classList.add('Current');
            if (isSelected)
                cell.classList.add('Selected');
            cell.textContent = date.getDate().toString();
            let daydate = new Date(date);
            cell.addEventListener('click', () => this.SelectDate(daydate));
            if (date.getDay() === 0)
                cell.classList.add('Sunday');
            if (date.getDay() === 6)
                cell.classList.add('Saturday');
            this.DaysGrid.HTML.appendChild(cell);
            date.setDate(date.getDate() + 1);
        }
    }
    SelectDate(pDate) {
        this.SelectedDate = pDate;
        this.ViewDate = new Date(pDate);
        this.UpdateCalendar();
        if (this.OnSelectdate != null)
            this.OnSelectdate.apply(this, [pDate]);
    }
    Navigate(pDirection) {
        switch (this.CurrentPanel) {
            case 'days':
                this.ViewDate.setMonth(this.ViewDate.getMonth() + pDirection);
                break;
            case 'months':
                this.ViewDate.setFullYear(this.ViewDate.getFullYear() + pDirection);
                break;
            default:
                this.ViewDate.setFullYear(this.ViewDate.getFullYear() + (pDirection * 16));
                break;
        }
        this.UpdateCalendar();
    }
    UpdateCalendar() {
        this.DaysGrid.IsVisible = false;
        this.MonthsGrid.IsVisible = false;
        this.YearsGrid.IsVisible = false;
        switch (this.CurrentPanel) {
            case 'days':
                this.ShowDays();
                this.CenterButton.SetContent(this.ViewDate.toLocaleDateString('pt-BR', { month: 'long', year: 'numeric' }));
                break;
            case 'months':
                this.ShowMonths();
                this.CenterButton.SetContent(this.ViewDate.getFullYear().toString());
                break;
            default:
                this.ShowYears();
                const year = this.ViewDate.getFullYear() - (this.ViewDate.getFullYear() % 16);
                this.CenterButton.SetContent(`${year} - ${year + 15}`);
                break;
        }
    }
    CreateContainer() {
        return XUtils.AddElement(null, "div", null);
    }
}
/// <reference path="XDiv.ts" />
class XMenuButtonItem extends XDiv {
    constructor(pOwner, pItem) {
        super(pOwner, "hover-item");
        this.HTML.textContent = pItem;
    }
}
class XHoverPanel extends XDiv {
    constructor(pOwner, pItem) {
        super(pOwner, "hover-panel");
        this.Header = new XDiv(this, 'accordion-header');
        const icon = new XDiv(this.Header, 'icon');
        icon.HTML.innerHTML = pItem.icon;
        const headerText = XUtils.AddElement(this.Header, "span", null);
        headerText.textContent = pItem.title;
    }
}
class XMenuItem extends XDiv {
    constructor(pOwner, pItem) {
        super(pOwner, "accordion-item");
        this.Menu = null;
        this.HoverPanel = null;
        this.HoverItens = new XArray();
        this.Title = null;
        this.Instances = null;
        this.Header = new XDiv(this, 'accordion-header');
        this.DataItem = pItem;
        this.Header.HTML.addEventListener('click', () => { var _a; return (_a = this.Menu) === null || _a === void 0 ? void 0 : _a.ExpandItem(this); });
        const icon = new XDiv(this.Header, 'icon');
        icon.HTML.innerHTML = pItem.icon;
        const headerText = XUtils.AddElement(this.Header, "span", "menu-span");
        headerText.textContent = pItem.title;
        this.CreateHoverPanel();
        this.CreateItens();
    }
    CreateItens() {
        if (this.DataItem.subItems) {
            const submenu = XUtils.AddElement(this, 'ul', 'accordion-submenu');
            if (this.DataItem.subItems.length > 8)
                submenu.classList.add('has-scroll');
            for (var i = 0; i < this.DataItem.subItems.length; i++) {
                var subitem = this.DataItem.subItems[i];
                const li = XUtils.AddElement(submenu, 'li', "XAppItem");
                this.Title = XUtils.AddElement(li, 'span', null);
                this.Instances = XUtils.AddElement(li, 'span', "XAppCount");
                this.Title.innerText = subitem;
                this.Instances.innerText = "(5)";
            }
            ;
        }
    }
    CreateHoverPanel() {
        if (this.DataItem.subItems) {
            this.HoverPanel = new XHoverPanel(this, this.DataItem);
            for (var i = 0; i < this.DataItem.subItems.length; i++) {
                var subitem = this.DataItem.subItems[i];
                var hitem = new XMenuButtonItem(this.HoverPanel, subitem);
                this.HoverItens.Add(hitem);
            }
        }
    }
}
class XMenu extends XDiv {
    constructor(pOwner) {
        super(pOwner, "XMenu");
        this.menuData = [
            {
                icon: '🏠',
                title: 'Home',
                subItems: ['Nossa História', 'Equipe', 'Parceiros']
            },
            {
                icon: '🛠️',
                title: 'Serviços',
                subItems: Array.from({ length: 10 }, (_, i) => `Serviço ${i + 1}`)
            },
            {
                icon: '📚',
                title: 'Sobre',
                subItems: ['Nossa História', 'Equipe', 'Parceiros']
            },
            {
                icon: '📦',
                title: 'Produtos',
                subItems: Array.from({ length: 12 }, (_, i) => `Produto ${i + 1}`)
            },
            {
                icon: '📞',
                title: 'Contato',
                subItems: ['Nossa História', 'Equipe', 'Parceiros']
            }
        ];
        this.Itens = new XArray();
        this.ToggleButton = new XBaseButton(this, "collapse-toggle");
        this.AccordionMenu = new XDiv(this, "accordion-menu");
        this.ToggleButton.HTML.addEventListener('click', (e) => this.Collaspse(e));
        this.CreateItens();
    }
    ExpandItem(pItem) {
        if (this.AccordionMenu.HTML.classList.contains('collapsed'))
            return;
        if (this.UnExpand(pItem))
            return;
        //if (this.AccordionMenu.HTML.classList.contains('collapsed'))
        //    this.AccordionMenu.HTML.classList.remove('collapsed');
        this.Itens.forEach(i => i.HTML.classList.remove('active'));
        if (pItem.DataItem.subItems)
            pItem.HTML.classList.add('active');
    }
    UnExpand(pItem = null) {
        var ret = false;
        if (pItem != null && !pItem.HTML.classList.contains('active'))
            return ret;
        this.Itens.forEach(i => i.HTML.classList.remove('active'));
        return true;
    }
    Collaspse(pArg) {
        this.UnExpand();
        this.AccordionMenu.HTML.classList.toggle('collapsed');
        this.HTML.classList.toggle('Collapsed');
    }
    CreateItens() {
        for (var i = 0; i < this.menuData.length; i++) {
            var mitem = this.menuData[i];
            var item = new XMenuItem(this.AccordionMenu, mitem);
            item.Menu = this;
            this.Itens.Add(item);
        }
    }
}
/// <reference path="XElement.ts" />
class XBaseButton extends XElement {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
    }
    CreateContainer() {
        return XUtils.AddElement(null, "div", null);
    }
}
/// <reference path="XBaseButton.ts" />
class XBaseTextButton extends XBaseButton {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
        this.Text = XUtils.AddElement(this, "span");
    }
    get Title() {
        return this.Text.innerHTML;
    }
    set Title(pValue) {
        this.Text.innerHTML = pValue;
    }
}
/// <reference path="Base/XBaseTextButton.ts" />
/// <reference path="Base/XPopupElement.ts" />
class XTabControlButton extends XBaseTextButton {
    constructor(pOwner) {
        super(pOwner, "XTabControlButton");
        this.TabControl = null;
        this.Tab = null;
        XEventManager.AddEvent(this, this.HTML, XEventType.Click, () => {
            var _a;
            (_a = this.TabControl) === null || _a === void 0 ? void 0 : _a.SelectTab(this);
        });
    }
}
class XTabControlHeader extends XDiv {
    constructor(pOwner) {
        super(pOwner, "XTabControlHeader");
        this.DropdownButton = null;
    }
    SelectionChanged() {
        this.ValidateVisibility();
    }
    SizeChanged() {
        if (this.DropdownButton != null)
            this.DropdownButton.IsVisible = this.HTML.scrollWidth > this.HTML.offsetWidth;
        this.ValidateVisibility();
    }
    ValidateVisibility() {
        const painelRect = this.HTML.getBoundingClientRect();
        this.HTML.childNodes.forEach(item => {
            var elm = item;
            const rect = elm.getBoundingClientRect();
            if (rect.left < painelRect.left || rect.right > painelRect.right)
                elm.style.visibility = 'hidden';
            else
                elm.style.visibility = 'visible';
        });
    }
}
class XTabControlTab extends XDiv {
    constructor(pOwner) {
        super(pOwner, "XTabControlTab");
        this.Button = null;
        this.IsDialogContainer = true;
        this.DialogContainer = new XDialogContainer(this, "XDialogContainer");
    }
}
class XTabControlContainer extends XDiv {
    constructor(pOwner) {
        super(pOwner, "XTabControlContainer");
    }
}
class XTabControlDropdown extends XPopupElement {
    constructor(pOwner) {
        super(pOwner, "XTabControlDropdown");
        this.HTML.addEventListener('wheel', function (event) {
            const { deltaY } = event;
            const { scrollTop, scrollHeight, clientHeight } = this;
            if ((deltaY > 0 && (scrollTop + clientHeight >= scrollHeight)) || (deltaY < 0 && scrollTop <= 0))
                event.preventDefault();
        });
    }
}
class XTabControlButtonList extends XBaseTextButton {
    constructor(pOwner) {
        super(pOwner, "XTabControlButtonList");
        this.IsVisible = false;
    }
}
class XTabControl extends XDiv {
    constructor(pOwner) {
        super(pOwner, "XTabControl");
        this.ActiveTab = null;
        this.Tabs = new XArray();
        this.IsDialogContainer = false;
        this.IsDialogContainer = true;
        this.DialogContainer = new XDialogContainer(this, "XDialogContainer");
        this.Header = new XTabControlHeader(this);
        this.Container = new XTabControlContainer(this);
        this.Dropdown = new XTabControlDropdown(this);
        XPopupManager.Add(this.Dropdown);
        this.ButtonList = new XTabControlButtonList(this);
        this.ButtonList.Title = "Abas";
        this.Header.DropdownButton = this.ButtonList;
        this.ButtonList.HTML.addEventListener('click', () => {
            this.PopulateDropdown();
        });
    }
    PopulateDropdown() {
        this.Dropdown.HTML.innerHTML = '';
        this.Tabs.forEach((tab, index) => {
            var _a, _b;
            const item = new XTabControlButton(this.Dropdown);
            item.HTML.className = "XTabControlButtonL";
            if (tab.Button != null) {
                if ((_a = tab === null || tab === void 0 ? void 0 : tab.Button) === null || _a === void 0 ? void 0 : _a.HTML.classList.contains('Active'))
                    item.HTML.classList.add('Active');
                item.TabControl = tab.Button.TabControl;
                item.Tab = tab.Button.Tab;
                item.Title = (_b = tab.Button) === null || _b === void 0 ? void 0 : _b.Title;
            }
        });
        this.Dropdown.IsVisible = true;
    }
    SelectTab(pButton) {
        var _a, _b, _c, _d, _e, _f, _g;
        if (pButton == null)
            return;
        this.Tabs.ForEach(t => {
            var _a;
            if (t.Button != null && t.Button.Tab != null) {
                (_a = t.Button) === null || _a === void 0 ? void 0 : _a.HTML.classList.remove('Active');
                t.Button.Tab.IsVisible = false;
            }
        });
        if (pButton != null && pButton.Tab != null && pButton.Tab.Button != null) {
            pButton.Tab.Button.HTML.classList.add('Active');
            pButton.HTML.classList.add('Active');
            pButton.Tab.IsVisible = true;
        }
        var rbtn = (_b = (_a = pButton === null || pButton === void 0 ? void 0 : pButton.Tab) === null || _a === void 0 ? void 0 : _a.Button) === null || _b === void 0 ? void 0 : _b.HTML.getBoundingClientRect();
        var rctn = this.Header.HTML.getBoundingClientRect();
        var offw = (_g = (_f = (_e = (_d = (_c = pButton === null || pButton === void 0 ? void 0 : pButton.Tab) === null || _c === void 0 ? void 0 : _c.Button) === null || _d === void 0 ? void 0 : _d.HTML) === null || _e === void 0 ? void 0 : _e.previousElementSibling) === null || _f === void 0 ? void 0 : _f.offsetWidth) !== null && _g !== void 0 ? _g : 0;
        if (rbtn != null) {
            if (rbtn.left < rctn.left)
                this.Header.HTML.scrollLeft -= (rctn.left - rbtn.left) + offw;
            else if (rbtn.right > rctn.right)
                this.Header.HTML.scrollLeft += (rbtn.right - rctn.right) + offw;
        }
        this.Dropdown.IsVisible = false;
        this.ActiveTab = pButton.Tab;
        this.Header.SelectionChanged();
    }
    AddTab(pTitle) {
        var _a;
        var btn = new XTabControlButton(this.Header);
        btn.Title = pTitle;
        btn.TabControl = this;
        var tab = this.CreateTab();
        tab.Button = btn;
        btn.Tab = tab;
        this.Tabs.Add(tab);
        tab.IsVisible = false;
        if (this.ActiveTab == null)
            this.SelectTab((_a = this.Tabs.FirstOrNull()) === null || _a === void 0 ? void 0 : _a.Button);
    }
    CreateTab() {
        return new XTabControlTab(this.Container);
        ;
    }
}
/// <reference path="../XDiv.ts" />
class XTableElement extends XElement {
    constructor(pOwner, pClass = null, pTag = null) {
        super(pOwner, pClass, pTag);
        this.Cells = new XArray();
    }
    AddCell(pClass) {
        var cell = new XTableElement(this, pClass, "tr");
        this.Cells.Add(cell);
    }
    CreateContainer(pTag = null) {
        return XUtils.AddElement(null, pTag, null);
    }
}
class XDragUtils {
    static SetData(pData) {
        this._Data = pData;
    }
    static GetData() {
        return this._Data;
    }
}
class XTableHCell extends XTableElement {
    constructor(pOwner, pClass = null) {
        super(pOwner, pClass, "th");
        this.Data = null;
        this.HRow = pOwner;
        this.Table = this.HRow.Header.Table;
        this.Content = XUtils.AddElement(this, "div", "XTableHContent");
        this.Sizer = XUtils.AddElement(this.Content, "div", "XTableResizer");
        this.TextArea = XUtils.AddElement(this.Content, "div", "XTableHTitle");
        this.Title = XUtils.AddElement(this.TextArea, "span");
        this.SortIcon = XUtils.AddElement(this.TextArea, "span", "sort-icon");
        this.SortState = { Field: "", Direction: 'asc' };
        this.ResizerEvents();
        this.DragEvents();
    }
    SetData(pCell) {
        this.SortState = { Field: "", Direction: 'asc' };
        this.Data = pCell;
        this.Title.innerHTML = "<spans>" + this.Data.Title + "</span>";
    }
    DragEvents() {
        this.Content.addEventListener('click', (e) => {
            if (e.target == this.Sizer)
                return;
            var act = 0;
            if (e.ctrlKey)
                act = 1;
            if (e.ctrlKey && e.shiftKey)
                act = 2;
            this.Table.Body.SortData(this, act);
            this.Table.ResizeColumn(this, this.HTML.GetRect().Width);
        });
        this.HTML.draggable = true;
        this.HTML.addEventListener('dragstart', (e) => {
            XDragUtils.SetData(this);
            this.HTML.classList.add('dragging');
        });
        this.HTML.addEventListener('dragend', (e) => {
            this.HTML.classList.remove('dragging');
        });
        this.HTML.addEventListener('dragover', (e) => {
            e.preventDefault();
            var elm = XDragUtils.GetData();
            if (elm == null || elm.UUID == this.UUID)
                return;
            var w = this.HTML.GetRect().Width;
            if (e.offsetX <= 5 || e.offsetX + 6 >= w)
                return;
            this.HTML.classList.remove('ldrag-over');
            this.HTML.classList.remove('rdrag-over');
            if (e.offsetX > w / 2)
                this.HTML.classList.add('rdrag-over');
            else
                this.HTML.classList.add('ldrag-over');
        });
        this.HTML.addEventListener('dragleave', () => {
            this.HTML.classList.remove('ldrag-over');
            this.HTML.classList.remove('rdrag-over');
        });
        this.HTML.addEventListener('drop', (e) => {
            e.preventDefault();
            this.HTML.classList.remove('ldrag-over');
            this.HTML.classList.remove('rdrag-over');
            const elm = XDragUtils.GetData();
            if (this.Owner instanceof XElement && elm.UUID != this.UUID) {
                var w = this.HTML.clientWidth / 2;
                if (e.offsetX > w)
                    this.MoveTo(this, elm);
                else
                    this.MoveTo(elm, this);
            }
        });
    }
    MoveTo(pLeft, pRight) {
        if (this.Owner instanceof XElement) {
            this.Owner.HTML.insertBefore(pLeft.HTML, pRight.HTML);
            this.Table.MoveTo(pLeft, pRight);
        }
    }
    ResizerEvents() {
        let isResizing = false;
        let startX = 0;
        let startWidth = 0;
        this.Sizer.addEventListener('mousedown', (e) => {
            e.stopPropagation();
            e.preventDefault();
            isResizing = true;
            startX = e.clientX;
            startWidth = this.Content.offsetWidth;
            const handleMouseMove = (e) => {
                if (!isResizing)
                    return;
                const newWidth = startWidth + (e.clientX - startX);
                this.Content.style.width = `${newWidth}px`;
                this.Data.Width = newWidth;
                this.Table.ResizeColumn(this, newWidth);
            };
            const handleMouseUp = () => {
                isResizing = false;
                document.removeEventListener('mousemove', handleMouseMove);
                document.removeEventListener('mouseup', handleMouseUp);
                this.Table.ResizeColumn(this, this.Content.GetRect().Width, true);
            };
            document.addEventListener('mousemove', handleMouseMove);
            document.addEventListener('mouseup', handleMouseUp, { once: true });
        });
    }
}
class XTableHRow extends XTableElement {
    constructor(pOwner) {
        super(pOwner, null, "tr");
        this.Header = pOwner;
    }
}
class XTableHeader extends XElement {
    constructor(pOwner, pTable) {
        super(pOwner, "XTableHeader");
        this.Columns = new XArray();
        this.TRows = new XTableHRow(this);
        this.Table = pTable;
    }
    Clear() {
        this.TRows.HTML.innerHTML = "";
    }
    AddColumns(pClass) {
        var cell = new XTableHCell(this.TRows, pClass);
        cell.Table = this.Table;
        this.Columns.Add(cell);
        return cell;
    }
    CreateContainer() {
        return XUtils.AddElement(null, "thead", null);
    }
}
class XTableBody extends XElement {
    constructor(pOwner, pTable) {
        super(pOwner, "");
        this.DataRows = new XArray();
        this.SortCells = new Array();
        this.Table = pTable;
        this.BRows = new XTableRow(this);
    }
    SortData(pCell, pAction) {
        switch (pAction) {
            case 0:
                this.SortCells = new Array();
                break;
            case 2:
                this.SortCells.Remove(pCell);
                break;
        }
        if (!this.SortCells.Any(c => c == pCell) && pAction != 2)
            this.SortCells.Add(pCell);
        let field = pCell.Data.Title;
        this.Table.Header.Columns.ForEach(c => {
            if (!this.SortCells.Any(cc => cc == c)) {
                c.SortIcon.innerHTML = "";
                c.Table.ResizeColumn(c, c.HTML.GetRect().Width);
            }
        });
        if (pAction != 2) {
            if (!X.IsEmpty(pCell.SortIcon.innerHTML))
                pCell.SortState.Direction = pCell.SortState.Direction === 'asc' ? 'desc' : 'asc';
            else
                pCell.SortState = { Field: field, Direction: 'asc' };
            pCell.SortIcon.innerHTML = pCell.SortState.Direction === 'asc' ? ' ▲' : ' ▼';
        }
        this.DataRows.sort((a, b) => {
            for (var i = 0; i < this.SortCells.length; i++) {
                let cell = this.SortCells[i];
                if (a.Tupla[cell.Data.Title] > b.Tupla[cell.Data.Title])
                    return cell.SortState.Direction === 'asc' ? 1 : -1;
                if (a.Tupla[cell.Data.Title] < b.Tupla[cell.Data.Title])
                    return cell.SortState.Direction === 'asc' ? -1 : 1;
            }
            return 0;
        });
        while (this.HTML.firstChild)
            this.HTML.removeChild(this.HTML.firstChild);
        for (var i = 0; i < this.DataRows.length; i++) {
            var row = this.DataRows[i];
            if (i % 2 != 0)
                row.HTML.className = "XTableRowEven";
            else
                row.HTML.className = "XTableRow";
            this.HTML.appendChild(row.HTML);
        }
    }
    Clear() {
        this.HTML.innerHTML = "";
    }
    AddRow() {
        var row = new XTableRow(this);
        this.DataRows.Add(row);
        return row;
    }
    CreateContainer() {
        return XUtils.AddElement(null, "tbody", null);
    }
}
class XTableRow extends XTableElement {
    constructor(pOwner) {
        super(pOwner, "XTableRow", "tr");
        this.Cell = new XArray();
        this.Body = pOwner;
        this.Table = pOwner.Table;
        XEventManager.AddEvent(this.Table, this.HTML, XEventType.Click, () => this.Table.DoSelectRow(this));
    }
    SetData(pTupla) {
        this.Tupla = pTupla;
        this.CreateCell();
    }
    CreateCell() {
        if (this.Table.Columns == null)
            return;
        for (var i = 0; i < this.Table.Columns.length; i++) {
            let cell = new XTableCell(this, "XTd");
            cell.SetData(this.Tupla[this.Table.Columns[i].Title], this.Table.Header.Columns[i]);
            this.Cell.Add(cell);
        }
    }
}
class XTableCell extends XTableElement {
    constructor(pOwner, pClass) {
        super(pOwner, pClass, "td");
        this.Content = XUtils.AddElement(this, "div", "XTableCellContent");
        this.Table = pOwner.Body.Table;
        this.Row = pOwner;
        this.Text = XUtils.AddElement(this.Content, "div", "XTableCellTitle");
    }
    SetData(pData, pHCell) {
        this.HCell = pHCell;
        this.Data = pData;
        this.Text.innerHTML = "<spans>" + this.Data + "</span>";
    }
}
class XTable extends XDiv {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
        this.Columns = null;
        this.DataSet = [];
        this.RowNumberColumn = { Title: '#', Visible: true, Width: 50 };
        this.OnRowClick = null;
        this.Container = XUtils.AddElement(this, "table");
        this.Header = new XTableHeader(this.Owner, this);
        this.Body = new XTableBody(this.Container, this);
        XEventManager.AddEvent(this, this.HTML, XEventType.Scroll, this.PositioningHeader);
    }
    DoSelectRow(pRow) {
        if (this.OnRowClick != null)
            this.OnRowClick.apply(this, [[pRow]]);
    }
    PositioningHeader(pArg) {
        this.Header.HTML.style.left = `-${this.HTML.scrollLeft}px`;
    }
    ResizeColumn(pHeaderCell, pWidth, pCheck = false) {
        var dcell = this.Body.DataRows[0].Cell.FirstOrNull(c => c.HCell == pHeaderCell);
        if (dcell != null) {
            if (pCheck) {
                if (pWidth > dcell.Content.clientWidth)
                    dcell.Content.style.width = `${pWidth}px`;
                else
                    pHeaderCell.Content.style.width = `${dcell.Content.GetRect().Width}px`;
            }
            else
                dcell.Content.style.width = `${pWidth}px`;
        }
    }
    MoveTo(pLeft, pRight) {
        if (this.Columns == null)
            return;
        var left = this.Body.DataRows[0].Cell.IndexOf(c => c.HCell == pLeft);
        var right = this.Body.DataRows[0].Cell.IndexOf(c => c.HCell == pRight);
        for (var i = 0; i < this.Body.DataRows.length; i++) {
            var row = this.Body.DataRows[i];
            var cl = row.Cell[left];
            var cr = row.Cell[right];
            row.HTML.insertBefore(cl.HTML, cr.HTML);
        }
    }
    GetVisibleColumns() {
        if (this.Columns == null)
            return new Array();
        return [this.RowNumberColumn, ...this.Columns.filter(c => c.Visible)];
    }
    SetDataSet(pDataSet) {
        this.DataSet = pDataSet;
        const fields = Object.keys(this.DataSet[0] || {});
        this.Columns = fields.map(Title => ({ Title, Visible: true, Width: 120 }));
        this.CreateHeader();
        this.CreateBody();
    }
    CreateBody() {
        this.Body.Clear();
        if (this.Columns == null)
            return;
        for (var i = 0; i < this.DataSet.length; i++) {
            let row = this.Body.AddRow();
            if (i % 2 != 0)
                row.HTML.className = "XTableRowEven";
            row.SetData(this.DataSet[i]);
        }
        XEventManager.SetTiemOut(this, this.AdjustCollumnWidth, 100);
    }
    AdjustCollumnWidth() {
        if (this.Body.DataRows.length > 0) {
            var row = this.Body.DataRows[0];
            for (var i = 0; i < row.Cell.length; i++) {
                let bcell = row.Cell[i];
                let hcell = this.Header.Columns[i];
                let bw = bcell.HTML.clientWidth;
                let hw = hcell.HTML.clientWidth;
                if (bw > hw)
                    hcell.Content.style.width = `${bw}px`;
                else
                    bcell.Content.style.width = `${hw}px`;
            }
        }
    }
    CreateHeader() {
        this.Body.Clear();
        if (this.Columns == null)
            return;
        for (var i = 0; i < this.Columns.length; i++) {
            let col = this.Columns[i];
            let cell = this.Header.AddColumns("XTh");
            cell.SetData(col);
        }
    }
}
class XType1 {
    constructor() {
        this.Point = new XPoint();
        this.LeftX = 0;
        this.LeftY = 0;
        this.Used = false;
        this.EndX = -1;
        this.StartX = -1;
    }
}
class XEditPosition {
    constructor(pLocation) {
        this.Used = false;
        this.Point = pLocation;
    }
}
class XForm extends XDiv {
    constructor(pOwner) {
        super(pOwner, "XForm");
        this.Fields = new XArray();
        var edt;
        edt = new XDatePickerEditor(this);
        edt.Rows = 1;
        edt.Cols = 9;
        edt.OrderIndex = 1;
        this.Fields.Add(edt);
        edt = new XMemoEditor(this);
        edt.Rows = 3;
        edt.Cols = 9;
        edt.OrderIndex = 2;
        this.Fields.Add(edt);
        edt = new XNormalEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 3;
        this.Fields.Add(edt);
        edt = new XIntegerEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 3;
        this.Fields.Add(edt);
        edt = new XIntegerEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 3;
        edt.Mask = "-##.##0";
        this.Fields.Add(edt);
        edt = new XIntegerEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 3;
        edt.Mask = "####0";
        this.Fields.Add(edt);
        edt = new XDecimalEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 3;
        edt.Mask = "####0";
        this.Fields.Add(edt);
        edt = new XDecimalEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 3;
        edt.AllowNegative = true;
        edt.MaxIntegerDigits = 4;
        edt.DecimalDigits = 2;
        this.Fields.Add(edt);
        edt = new XDecimalEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 3;
        edt.AllowNegative = true;
        edt.MaxIntegerDigits = 4;
        edt.DecimalDigits = 2;
        this.Fields.Add(edt);
        edt = new XEMailEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 3;
        this.Fields.Add(edt);
        edt = new XPhoneEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 3;
        this.Fields.Add(edt);
        edt = new XPhoneEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 3;
        this.Fields.Add(edt);
        edt = new XDataLoockupEditor(this);
        edt.Rows = 1;
        edt.Cols = 8;
        edt.OrderIndex = 3;
        this.Fields.Add(edt);
        edt = new XDataGridEditor(this);
        edt.Rows = 7;
        edt.Cols = 32;
        edt.OrderIndex = 3;
        this.Fields.Add(edt);
        edt = new XButtonEditor(this);
        edt.Rows = 1;
        edt.Cols = 1;
        edt.OrderIndex = 3;
        this.Fields.Add(edt);
        var cn = 1;
        this.Fields.ForEach(e => e.OrderIndex = cn++);
        this.Fields = this.Fields.OrderBy(e => e.OrderIndex);
    }
    SizeChanged() {
        this.ResizeChildren();
    }
    ResizeChildren() {
        const cols = XDefault.DefaultColCount;
        const rows = 80;
        const cellw = this.HTML.GetRect(true).Width / cols;
        const cellh = XDefault.DefaultRowHeight;
        const ordered = this.Fields.OrderBy(c => c.OrderIndex);
        const grid = Array.from({ length: rows }, () => new Array(cols).fill(false));
        for (const child of ordered) {
            const ccols = child.Cols;
            const crows = child.Rows;
            if (ccols > cols || crows > rows)
                continue;
            let placed = false;
            for (let row = 0; row <= rows - crows; row++) {
                for (let col = 0; col <= cols - ccols; col++) {
                    let fplace = true;
                    for (let r = row; r < row + crows; r++) {
                        for (let c = col; c < col + ccols; c++) {
                            if (grid[r][c]) {
                                fplace = false;
                                break;
                            }
                        }
                        if (!fplace)
                            break;
                    }
                    if (fplace) {
                        for (let r = row; r < row + crows; r++)
                            for (let c = col; c < col + ccols; c++)
                                grid[r][c] = true;
                        const x = col * cellw;
                        const y = row * cellh;
                        var r = new XRect(x, y, ccols * cellw, crows * cellh);
                        r.Inflate(-2, -2);
                        child.Rect = r;
                        placed = true;
                        break;
                    }
                }
                if (placed)
                    break;
            }
        }
        var tidx = 1;
        var tabs = this.SortRectangles(this.Fields);
        for (const child of tabs)
            child.Input.tabIndex = tidx++;
    }
    SortRectangles(rectangles) {
        return rectangles.sort((a, b) => {
            if (a.Rect.Top < b.Rect.Top)
                return -1;
            if (a.Rect.Top > b.Rect.Top)
                return 1;
            if (a.Rect.Left < b.Rect.Left)
                return -1;
            if (a.Rect.Left > b.Rect.Left)
                return 1;
            return 0;
        });
    }
}
/// <reference path="../Elements/XDiv.ts" />
class XStage extends XDiv {
    static Run() {
        window.onmousedown = (arg) => XPopupManager.HideAll(arg);
        window.onkeydown = (a) => XHotkeyManager.OnKeyDown(a);
        this.Instance = new XStage();
    }
    constructor() {
        super(document.body, "MainDiv");
        this.Menu = new XMenu(this);
        this.TopBar = new XTopBar(this);
        this.Menu.OnResize = () => this.MenuResize();
        this.TabControl = new XStageTabControl(this);
        this.TabControl.Dropdown.HTML.classList.add("Main");
    }
    SizeChanged() {
        this.MenuResize();
    }
    MenuResize() {
        var r = this.Menu.HTML.GetRect();
        this.TabControl.HTML.style.left = `${r.Width}px`;
        this.TabControl.HTML.style.width = `${this.Rect.Width - r.Width - 1}px`;
        this.TopBar.HTML.style.left = `${r.Width}px`;
        this.TopBar.HTML.style.width = `${this.Rect.Width - r.Width - 1}px`;
    }
}
class XStageTabControlTab extends XTabControlTab {
    constructor(pOwner) {
        super(pOwner);
        this.IsDialogContainer = true;
        this.Form = new XForm(this);
    }
}
class XStageTabControl extends XTabControl {
    constructor(pOwner) {
        super(pOwner);
        this.IsDialogContainer = true;
        this.HTML.classList.add("Main");
        this.AddTab("Aninha");
        this.AddTab("Maria");
        //this.AddTab("Joana");
        //this.AddTab("Rebeca");
        //this.AddTab("Antonieta");
        //this.AddTab("Valentina");
        //this.AddTab("Amanda");
        //this.AddTab("Jaqueline");
        //this.AddTab("Helena");
        //this.AddTab("Fernanda");
        //this.AddTab("Sonia");
        //this.AddTab("Larissa");
        //this.AddTab("Eleonora");
        //this.AddTab("Sara");
        //this.AddTab("Sebastina");
        //this.AddTab("Sabrina");
    }
    CreateTab() {
        return new XStageTabControlTab(this.Container);
        ;
    }
}
class XTopBar extends XDiv {
    constructor(pOwner) {
        super(pOwner, "XTopBar");
    }
}
/// <reference path="src/XDefault.ts" />
/// <reference path="src/XConst.ts" />
/// <reference path="src/XInterfaces.ts" />
/// <reference path="src/XExtensions.ts" />
/// <reference path="src/XMath.ts" />
/// <reference path="src/XSort.ts" />
/// <reference path="src/Utils/XUtils.ts" />
/// <reference path="src/XTypes.ts" />
/// <reference path="src/Elements/Base/XElement.ts" />
/// <reference path="src/XPopupManager.ts" />
/// <reference path="src/XEventManager.ts" />
/// <reference path="src/Elements/Base/XElement.ts" />
/// <reference path="src/Elements/XDiv.ts" />
/// <reference path="src/Elements/Base/XBaseButton.ts" />
/// <reference path="src/Elements/Base/XBaseTextButton.ts" />
/// <reference path="src/Elements/Base/XBaseInput.ts" />
/// <reference path="src/Elements/Base/XBaseButtonInput.ts" />
/// <reference path="src/Elements/Base/XPopupElement.ts" />
/// <reference path="src/Elements/Base/XDropDownElement.ts" />
/// <reference path="src/Elements/Base/XBaseLoockupInput.ts" />
/// <reference path="src/Elements/XMenu.ts" />
/// <reference path="src/Elements/Base/XTable.ts" />
/// <reference path="src/Elements/XTabControl.ts" />
/// <reference path="src/Elements/XCalendar.ts" />
/// <reference path="src/Elements/XDataGrid.ts" />
/// <reference path="src/Editors/XDatePickerEditor.ts" />
/// <reference path="src/Editors/XMemoEditor.ts" />
/// <reference path="src/Editors/XNormalEditor.ts" />
/// <reference path="src/Editors/XDataGridEditor.ts" />
/// <reference path="src/Stage/XStage.ts" />
/// <reference path="XDiv.ts" />
class XWrapPanel extends XDiv {
    constructor(pOwner, pClass = null) {
        super(pOwner, pClass !== null && pClass !== void 0 ? pClass : "XWrapPanel");
    }
}
/// <reference path="XSizeableElement.ts" />
/// <reference path="../XWrapPanel.ts" />
class XBaseDialogCaption extends XDiv {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
        this.ELMTitle = new XDiv(this, "XDialogTitle");
    }
    get Title() {
        return this.ELMTitle.HTML.innerHTML;
    }
    set Title(pValue) {
        this.ELMTitle.HTML.innerHTML = pValue;
    }
}
class XBaseButtonBar extends XWrapPanel {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
        this.Cancel = new XBaseTextButton(this, "XDialogButton");
        this.Cancel.Title = "Cancelar";
    }
}
class XBaseDialog extends XSizeableElement {
    constructor(pOwner) {
        var _a;
        super(pOwner, "XDialog");
        this.IsDialog = true;
        (_a = this.HTML.parentElement) === null || _a === void 0 ? void 0 : _a.removeChild(this.HTML);
        this.AutoIncZIndex = true;
        this.Caption = new XBaseDialogCaption(this, "XDialogCaption");
        this.ButtonBar = new XBaseButtonBar(this, "XButtonBar Right");
        XEventManager.AddEvent(this, this.ButtonBar.Cancel.HTML, XEventType.Click, this.Cancel);
    }
    Cancel(pArg) {
        if (this.HTML.parentElement == null)
            return;
        this.IsVisible = false;
    }
    get Title() {
        return this.Caption.Title;
    }
    set Title(pValue) {
        this.Caption.Title = pValue;
    }
    ShowDialog() {
        this.IsVisible = true;
        this.StartMouseDown(null);
    }
    StartMouseDown(pArg) {
        var r = this.Caption.HTML.GetRect();
        var lb = this.HTML.StyleValue("border-left");
        var tb = this.HTML.StyleValue("border-top");
        this.DragRect = new XRect(lb, tb, r.Width, r.Height);
    }
    IncZIndex() {
        this.HTML.style.zIndex = `${999 + XPopupManager.ZIndex()}`;
    }
    Show(pValue = true) {
        var _a;
        if (this._DialogContainer == null) {
            this._DialogContainer = this.GetDialogContainer();
            if (this._DialogContainer.HTML != this.HTML) {
                (_a = this.HTML.parentElement) === null || _a === void 0 ? void 0 : _a.removeChild(this.HTML);
                this._DialogContainer.DialogContainer.HTML.appendChild(this.HTML);
            }
        }
        super.Show(pValue);
        this._DialogContainer.DialogContainer.IsVisible = pValue;
    }
}
/// <reference path="../Elements/Base/XBaseInput.ts" />
class XButtonEditor extends XBaseInput {
    constructor(pOwner) {
        super(pOwner);
        this.Title = "Clique no Botão";
        XEventManager.AddEvent(this, this.Button.HTML, XEventType.Click, this.OnClick, true);
    }
    CreateInput() {
        this.Button = new XBaseButton(this, "XLookupButton");
        return this.Button.HTML;
    }
    OnClick(pArg) {
        var con = this.GetDialogContainer();
        var dlg = new XBaseDialog(con);
        dlg.Title = "Mostrando o Dialogo";
        dlg.ShowDialog();
    }
}
/// <reference path="XDiv.ts" />
class XDialogContainer extends XDiv {
    constructor(pOwner, pClass) {
        super(pOwner, pClass);
        this.IsVisible = false;
        this.AutoIncZIndex = true;
    }
}
//# sourceMappingURL=TFX.Core.js.map