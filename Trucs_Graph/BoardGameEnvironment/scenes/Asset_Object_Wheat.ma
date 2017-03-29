//Maya ASCII 2016 scene
//Name: Asset_Blé.ma
//Last modified: Wed, Mar 15, 2017 04:55:55 PM
//Codeset: 1252
requires maya "2016";
requires -nodeType "Unfold3DUnfold" "Unfold3D" "Trunk.r2232.release.Mar 18 2015|11:44:26";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2016";
fileInfo "version" "2016";
fileInfo "cutIdentifier" "201603180400-990260";
fileInfo "osv" "Microsoft Windows 8 Business Edition, 64-bit  (Build 9200)\n";
createNode transform -s -n "persp";
	rename -uid "9CFEAA4C-4B3E-1432-5D5B-1BB8152C664E";
	setAttr ".v" no;
	setAttr ".t" -type "double3" -211.22058253962769 84.497926769724813 42.545408283070856 ;
	setAttr ".r" -type "double3" -15.338352728249223 -91.79999999999572 0 ;
createNode camera -s -n "perspShape" -p "persp";
	rename -uid "749FFBFB-4B16-F096-F58A-028F1F407998";
	setAttr -k off ".v" no;
	setAttr ".fl" 34.999999999999993;
	setAttr ".coi" 217.89181553062369;
	setAttr ".imn" -type "string" "persp";
	setAttr ".den" -type "string" "persp_depth";
	setAttr ".man" -type "string" "persp_mask";
	setAttr ".hc" -type "string" "viewSet -p %camera";
createNode transform -s -n "top";
	rename -uid "534FB9B0-4FBC-6050-509C-48B2997858B6";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 0 100.1 0 ;
	setAttr ".r" -type "double3" -89.999999999999986 0 0 ;
createNode camera -s -n "topShape" -p "top";
	rename -uid "403472E9-46D6-8D7B-73BD-179F3F741910";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 100.1;
	setAttr ".ow" 30;
	setAttr ".imn" -type "string" "top";
	setAttr ".den" -type "string" "top_depth";
	setAttr ".man" -type "string" "top_mask";
	setAttr ".hc" -type "string" "viewSet -t %camera";
	setAttr ".o" yes;
createNode transform -s -n "front";
	rename -uid "414B920C-4A1E-2D6C-E50B-25B1A729BCDE";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 0 0 100.1 ;
createNode camera -s -n "frontShape" -p "front";
	rename -uid "677A7127-40B1-DAE9-BD5C-7CBA3011D3F9";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 100.1;
	setAttr ".ow" 30;
	setAttr ".imn" -type "string" "front";
	setAttr ".den" -type "string" "front_depth";
	setAttr ".man" -type "string" "front_mask";
	setAttr ".hc" -type "string" "viewSet -f %camera";
	setAttr ".o" yes;
createNode transform -s -n "side";
	rename -uid "AE1C2DC6-42F5-E78D-7CBB-9FBC6ED92738";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 100.1 0 0 ;
	setAttr ".r" -type "double3" 0 89.999999999999986 0 ;
createNode camera -s -n "sideShape" -p "side";
	rename -uid "A63C675C-47F5-CDC8-E477-8DBCC4D3DEC5";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 100.1;
	setAttr ".ow" 30;
	setAttr ".imn" -type "string" "side";
	setAttr ".den" -type "string" "side_depth";
	setAttr ".man" -type "string" "side_mask";
	setAttr ".hc" -type "string" "viewSet -s %camera";
	setAttr ".o" yes;
createNode transform -n "pCylinder1";
	rename -uid "19ADA17F-4AE5-C6D0-6E6D-2699E5DAD761";
	setAttr ".t" -type "double3" 0 1 0 ;
	setAttr ".s" -type "double3" 11.538247093128415 11.538247093128415 11.538247093128415 ;
	setAttr ".rp" -type "double3" 0 -1 0 ;
	setAttr ".sp" -type "double3" 0 -1 0 ;
createNode mesh -n "pCylinderShape1" -p "pCylinder1";
	rename -uid "4037F332-4E5A-885C-6D8C-4D90E76E9BDB";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.49957865476608276 0.50578542053699493 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode lightLinker -s -n "lightLinker1";
	rename -uid "DA7666BD-4D96-A806-E314-C986250A6CCA";
	setAttr -s 4 ".lnk";
	setAttr -s 4 ".slnk";
createNode displayLayerManager -n "layerManager";
	rename -uid "F25963EC-40F0-4222-1626-E8BAA5FCB0DC";
createNode displayLayer -n "defaultLayer";
	rename -uid "0EB804A4-47A6-3D3A-B4B2-919C4D4D1585";
createNode renderLayerManager -n "renderLayerManager";
	rename -uid "FA916B55-4357-5054-F1AE-A78A626376D7";
createNode renderLayer -n "defaultRenderLayer";
	rename -uid "31EDD602-4D73-F388-BAAC-9F893D94C2E7";
	setAttr ".g" yes;
createNode lambert -n "lambert5";
	rename -uid "F11036FE-4F0A-B8D7-C767-9BB4AFDC7AE6";
	setAttr ".c" -type "float3" 0.037433002 0.45100001 0.085151777 ;
createNode shadingEngine -n "BushSG";
	rename -uid "57256DA8-4BAF-DF33-3F20-2E9215A06EB0";
	setAttr ".ihi" 0;
	setAttr ".ro" yes;
createNode materialInfo -n "materialInfo1";
	rename -uid "9FC9050D-4F2B-D74F-A6BC-1A9692805F3F";
createNode phong -n "blinn1";
	rename -uid "48274461-4C5E-CCB5-EA1F-D3B6EB37A60E";
	setAttr ".c" -type "float3" 0 0.028372815 0.37 ;
	setAttr ".cp" 6.311790943145752;
createNode shadingEngine -n "BerrySG";
	rename -uid "3FC666BF-4D26-0A52-AF70-6391CC6A5571";
	setAttr ".ihi" 0;
	setAttr ".ro" yes;
createNode materialInfo -n "materialInfo2";
	rename -uid "429C4BA0-48C2-4850-AF1E-8AB78C74CC4A";
createNode polyCylinder -n "polyCylinder1";
	rename -uid "FC305BB8-4255-7B16-5465-178ABE2F86E4";
	setAttr ".sa" 8;
	setAttr ".sc" 1;
	setAttr ".cuv" 3;
createNode polySplit -n "polySplit1";
	rename -uid "8F518E31-4E4B-E589-674E-209C9317D42A";
	setAttr -s 9 ".e[0:8]"  0.65701401 0.65701401 0.65701401 0.65701401
		 0.65701401 0.65701401 0.65701401 0.65701401 0.65701401;
	setAttr -s 9 ".d[0:8]"  -2147483632 -2147483631 -2147483630 -2147483629 -2147483628 -2147483627 
		-2147483626 -2147483625 -2147483632;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySplit -n "polySplit2";
	rename -uid "3A1E675E-48A2-B7CD-1942-FAAE9871F2E1";
	setAttr -s 9 ".e[0:8]"  0.14751001 0.14751001 0.14751001 0.14751001
		 0.14751001 0.14751001 0.14751001 0.14751001 0.14751001;
	setAttr -s 9 ".d[0:8]"  -2147483608 -2147483601 -2147483602 -2147483603 -2147483604 -2147483605 
		-2147483606 -2147483607 -2147483608;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySplit -n "polySplit3";
	rename -uid "9F21526C-40A6-CD43-31B6-70BBAFB57144";
	setAttr -s 9 ".e[0:8]"  0.16628 0.16628 0.16628 0.16628 0.16628 0.16628
		 0.16628 0.16628 0.16628;
	setAttr -s 9 ".d[0:8]"  -2147483592 -2147483591 -2147483590 -2147483589 -2147483588 -2147483587 
		-2147483586 -2147483585 -2147483592;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySplit -n "polySplit4";
	rename -uid "F6C1A08A-4023-4685-E185-FD9B5A569F6D";
	setAttr -s 9 ".e[0:8]"  0.892932 0.892932 0.892932 0.892932 0.892932
		 0.892932 0.892932 0.892932 0.892932;
	setAttr -s 9 ".d[0:8]"  -2147483632 -2147483631 -2147483630 -2147483629 -2147483628 -2147483627 
		-2147483626 -2147483625 -2147483632;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polyTweak -n "polyTweak1";
	rename -uid "5055C460-44DF-6602-3580-91AFC9CD6550";
	setAttr ".uopa" yes;
	setAttr -s 41 ".tk";
	setAttr ".tk[8]" -type "float3" -0.5111264 0.15184295 0.51112652 ;
	setAttr ".tk[9]" -type "float3" 4.3084672e-008 0.15184295 0.72284079 ;
	setAttr ".tk[10]" -type "float3" 0.51112652 0.15184295 0.51112652 ;
	setAttr ".tk[11]" -type "float3" 0.72284079 0.15184295 2.1542336e-008 ;
	setAttr ".tk[12]" -type "float3" 0.51112652 0.15184295 -0.51112646 ;
	setAttr ".tk[13]" -type "float3" 4.3084672e-008 0.15184295 -0.72284079 ;
	setAttr ".tk[14]" -type "float3" -0.51112646 0.15184295 -0.51112652 ;
	setAttr ".tk[15]" -type "float3" -0.72284079 0.15184295 2.1542336e-008 ;
	setAttr ".tk[17]" -type "float3" 4.3084672e-008 0.15184295 2.1542336e-008 ;
	setAttr ".tk[18]" -type "float3" -0.51155823 0.62170041 0.51155919 ;
	setAttr ".tk[19]" -type "float3" 4.3121187e-008 0.62170041 0.72345346 ;
	setAttr ".tk[20]" -type "float3" 0.51155835 0.62170041 0.51155919 ;
	setAttr ".tk[21]" -type "float3" 0.72345346 0.62170041 1.1103841e-018 ;
	setAttr ".tk[22]" -type "float3" 0.51155835 0.62170041 -0.51155919 ;
	setAttr ".tk[23]" -type "float3" 4.3121187e-008 0.62170041 -0.72345346 ;
	setAttr ".tk[24]" -type "float3" -0.51155919 0.62170041 -0.51155835 ;
	setAttr ".tk[25]" -type "float3" -0.72345346 0.62170041 1.1103841e-018 ;
	setAttr ".tk[26]" -type "float3" -0.59456301 0.54992461 0.59456307 ;
	setAttr ".tk[27]" -type "float3" -0.84083915 0.54992461 1.1103841e-018 ;
	setAttr ".tk[28]" -type "float3" -0.59456307 0.54992461 -0.59456301 ;
	setAttr ".tk[29]" -type "float3" 5.0117919e-008 0.54992461 -0.84083903 ;
	setAttr ".tk[30]" -type "float3" 0.59456301 0.54992461 -0.59456307 ;
	setAttr ".tk[31]" -type "float3" 0.84083915 0.54992461 1.1103841e-018 ;
	setAttr ".tk[32]" -type "float3" 0.59456301 0.54992461 0.59456307 ;
	setAttr ".tk[33]" -type "float3" 5.0117919e-008 0.54992461 0.84083903 ;
	setAttr ".tk[34]" -type "float3" -0.51155823 0.47814876 0.51155919 ;
	setAttr ".tk[35]" -type "float3" -0.72345346 0.47814876 1.1103841e-018 ;
	setAttr ".tk[36]" -type "float3" -0.51155919 0.47814876 -0.51155835 ;
	setAttr ".tk[37]" -type "float3" 4.3121187e-008 0.47814876 -0.72345334 ;
	setAttr ".tk[38]" -type "float3" 0.51155835 0.47814876 -0.51155919 ;
	setAttr ".tk[39]" -type "float3" 0.72345346 0.47814876 1.1103841e-018 ;
	setAttr ".tk[40]" -type "float3" 0.51155835 0.47814876 0.51155919 ;
	setAttr ".tk[41]" -type "float3" 4.3121187e-008 0.47814876 0.72345334 ;
	setAttr ".tk[42]" -type "float3" 0 -0.13871169 0 ;
	setAttr ".tk[43]" -type "float3" -1.376454e-023 -0.13871169 0 ;
	setAttr ".tk[44]" -type "float3" 0 -0.13871169 0 ;
	setAttr ".tk[45]" -type "float3" 0 -0.13871169 0 ;
	setAttr ".tk[46]" -type "float3" 0 -0.13871169 0 ;
	setAttr ".tk[47]" -type "float3" -1.376454e-023 -0.13871169 0 ;
	setAttr ".tk[48]" -type "float3" 0 -0.13871169 0 ;
	setAttr ".tk[49]" -type "float3" 0 -0.13871169 0 ;
createNode polySplit -n "polySplit5";
	rename -uid "B0F0CB39-4230-3038-BBA2-A197E422E911";
	setAttr -s 9 ".e[0:8]"  0.171096 0.171096 0.171096 0.171096 0.171096
		 0.171096 0.171096 0.171096 0.171096;
	setAttr -s 9 ".d[0:8]"  -2147483632 -2147483631 -2147483630 -2147483629 -2147483628 -2147483627 
		-2147483626 -2147483625 -2147483632;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polyTweak -n "polyTweak2";
	rename -uid "A24AF880-4830-04EF-9126-E18A530AF4EB";
	setAttr ".uopa" yes;
	setAttr -s 8 ".tk[50:57]" -type "float3"  0.11840998 -0.078948796 -0.11840998
		 -9.9812203e-009 -0.078948796 -0.16745709 -0.11840998 -0.078948796 -0.11840998 -0.16745709
		 -0.078948796 -4.9906101e-009 -0.11840998 -0.078948796 0.11840998 -9.9812203e-009
		 -0.078948796 0.16745709 0.11840998 -0.078948796 0.11840998 0.16745709 -0.078948796
		 -4.9906101e-009;
createNode polySplit -n "polySplit6";
	rename -uid "1B415822-4CA6-4664-0EF2-7A88ADA57AF7";
	setAttr -s 9 ".e[0:8]"  0.66002601 0.66002601 0.66002601 0.66002601
		 0.66002601 0.66002601 0.66002601 0.66002601 0.66002601;
	setAttr -s 9 ".d[0:8]"  -2147483560 -2147483553 -2147483554 -2147483555 -2147483556 -2147483557 
		-2147483558 -2147483559 -2147483560;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polyTweak -n "polyTweak3";
	rename -uid "4C1985A0-4524-A46E-05F9-0BB40028DF49";
	setAttr ".uopa" yes;
	setAttr -s 66 ".tk[0:65]" -type "float3"  -0.14875419 6.3282712e-015
		 6.3282712e-015 0 6.3282712e-015 6.3282712e-015 0.14875419 6.3282712e-015 6.3282712e-015
		 0.21037017 6.3282712e-015 0 0.14875419 6.3282712e-015 -6.3282712e-015 0 6.3282712e-015
		 -6.3282712e-015 -0.14875419 6.3282712e-015 -6.3282712e-015 -0.21037018 6.3282712e-015
		 0 -0.041228402 -1.2656542e-014 1.5820678e-015 -9.0637302e-009 -1.2656542e-014 3.1641356e-015
		 0.04122838 -1.2656542e-014 1.5820678e-015 0.058306009 -1.2656542e-014 -1.8859718e-022
		 0.04122838 -1.2656542e-014 -1.5820678e-015 -9.0637302e-009 -1.2656542e-014 -3.1641356e-015
		 -0.041228402 -1.2656542e-014 -1.5820678e-015 -0.058306038 -1.2656542e-014 -1.8859718e-022
		 0 6.3282712e-015 0 -9.0637302e-009 -1.2656542e-014 -1.8859718e-022 -0.04113758 -6.3282712e-015
		 1.5820678e-015 -9.0714121e-009 -6.3282712e-015 3.1641356e-015 0.04113755 -6.3282712e-015
		 1.5820678e-015 0.058177117 -6.3282712e-015 -1.0977801e-032 0.04113755 -6.3282712e-015
		 -1.5820678e-015 -9.0714121e-009 -6.3282712e-015 -3.1641356e-015 -0.041137386 -6.3282712e-015
		 -1.5820678e-015 -0.058177155 -6.3282712e-015 -1.0977801e-032 -0.023675844 -6.3282712e-015
		 7.9103391e-016 -0.033482708 -6.3282712e-015 -1.0977801e-032 -0.023675844 -6.3282712e-015
		 -7.9103391e-016 -1.0543316e-008 -6.3282712e-015 -1.5820678e-015 0.023675844 -6.3282712e-015
		 -7.9103391e-016 0.033482663 -6.3282712e-015 -1.0977801e-032 0.023675844 -6.3282712e-015
		 7.9103391e-016 -1.0543316e-008 -6.3282712e-015 1.5820678e-015 -0.04113758 -6.3282712e-015
		 1.5820678e-015 -0.058177155 -6.3282712e-015 -1.0977801e-032 -0.041137386 -6.3282712e-015
		 -1.5820678e-015 -9.0714121e-009 -6.3282712e-015 -3.1641356e-015 0.04113755 -6.3282712e-015
		 -1.5820678e-015 0.058177091 -6.3282712e-015 -1.0977801e-032 0.04113755 -6.3282712e-015
		 1.5820678e-015 -9.0714121e-009 -6.3282712e-015 3.1641356e-015 -0.14875419 -3.9551695e-016
		 6.3282712e-015 2.8956492e-024 -3.9551695e-016 6.3282712e-015 0.14875419 -3.9551695e-016
		 6.3282712e-015 0.21037017 -3.9551695e-016 0 0.14875419 -3.9551695e-016 -6.3282712e-015
		 2.8956492e-024 -3.9551695e-016 -6.3282712e-015 -0.14875419 -3.9551695e-016 -6.3282712e-015
		 -0.21037018 -3.9551695e-016 0 -0.17366405 6.3282712e-015 6.3282712e-015 2.0997515e-009
		 6.3282712e-015 1.2656542e-014 0.17366405 6.3282712e-015 6.3282712e-015 0.24559811
		 6.3282712e-015 4.7149295e-023 0.17366405 6.3282712e-015 -6.3282712e-015 2.0997515e-009
		 6.3282712e-015 -1.2656542e-014 -0.17366405 6.3282712e-015 -6.3282712e-015 -0.24559811
		 6.3282712e-015 4.7149295e-023 0.036940306 -6.5503158e-015 -0.14521298 0.052241363
		 -6.5503158e-015 2.8804967e-019 0.036940198 -6.5503158e-015 0.14521326 -1.5652867e-008
		 -6.5503158e-015 0.20536226 -0.036940333 -6.5503158e-015 0.14521298 -0.052241359 -6.5503158e-015
		 2.8804967e-019 -0.036940333 -6.5503158e-015 -0.14521298 -1.5652867e-008 -6.5503158e-015
		 -0.20536226;
createNode polySplit -n "polySplit7";
	rename -uid "F696595D-4391-0BAA-6099-3091317F1403";
	setAttr -s 9 ".e[0:8]"  0.068089299 0.068089299 0.068089299 0.068089299
		 0.068089299 0.068089299 0.068089299 0.068089299 0.068089299;
	setAttr -s 9 ".d[0:8]"  -2147483576 -2147483575 -2147483574 -2147483573 -2147483572 -2147483571 
		-2147483570 -2147483569 -2147483576;
	setAttr ".sma" 180;
	setAttr ".m2015" yes;
createNode polySoftEdge -n "polySoftEdge1";
	rename -uid "287EF946-4037-D725-CA8B-55AEC2EC211F";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[*]";
	setAttr ".ix" -type "matrix" 11.538247093128415 0 0 0 0 11.538247093128415 0 0 0 0 11.538247093128415 0
		 0 11.538247093128415 0 1;
	setAttr ".a" 0;
createNode polyTweak -n "polyTweak4";
	rename -uid "37B9E37D-40FD-BF1B-0329-1AAE3B53058F";
	setAttr ".uopa" yes;
	setAttr -s 17 ".tk";
	setAttr ".tk[34]" -type "float3" -0.065245494 0.027859649 0.082627364 ;
	setAttr ".tk[35]" -type "float3" -0.092270553 0.027859649 -4.6918687e-019 ;
	setAttr ".tk[36]" -type "float3" -0.065245047 0.027859649 -0.08262793 ;
	setAttr ".tk[37]" -type "float3" 7.6498541e-009 0.027859649 -0.11685327 ;
	setAttr ".tk[38]" -type "float3" 0.065245494 0.027859649 -0.082627364 ;
	setAttr ".tk[39]" -type "float3" 0.092270553 0.027859649 -4.6918687e-019 ;
	setAttr ".tk[40]" -type "float3" 0.065245494 0.027859649 0.082627364 ;
	setAttr ".tk[41]" -type "float3" 7.6498541e-009 0.027859649 0.11685327 ;
	setAttr ".tk[66]" -type "float3" 0 0.047283739 0 ;
	setAttr ".tk[67]" -type "float3" 0 0.047283739 0 ;
	setAttr ".tk[68]" -type "float3" 0 0.047283739 0 ;
	setAttr ".tk[69]" -type "float3" -1.323489e-023 0.047283739 0 ;
	setAttr ".tk[70]" -type "float3" 0 0.047283739 0 ;
	setAttr ".tk[71]" -type "float3" 0 0.047283739 0 ;
	setAttr ".tk[72]" -type "float3" 0 0.047283739 0 ;
	setAttr ".tk[73]" -type "float3" -1.323489e-023 0.047283739 0 ;
createNode polyMergeUV -n "polyMergeUV1";
	rename -uid "6DF87243-460A-33A0-9397-AE914429C04D";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "map[0:98]";
	setAttr ".d" 100;
createNode polyTweak -n "polyTweak5";
	rename -uid "4574069E-43E3-4A04-F216-78AFCFE41E84";
	setAttr ".uopa" yes;
	setAttr -s 17 ".tk";
	setAttr ".tk[0]" -type "float3" -4.1078252e-015 4.1078252e-015 0.030376956 ;
	setAttr ".tk[1]" -type "float3" 0 4.1078252e-015 0.042959459 ;
	setAttr ".tk[2]" -type "float3" 4.1078252e-015 4.1078252e-015 0.030376956 ;
	setAttr ".tk[3]" -type "float3" 4.1078252e-015 4.1078252e-015 0 ;
	setAttr ".tk[4]" -type "float3" 4.1078252e-015 4.1078252e-015 -0.030376956 ;
	setAttr ".tk[5]" -type "float3" 0 4.1078252e-015 -0.042959489 ;
	setAttr ".tk[6]" -type "float3" -4.1078252e-015 4.1078252e-015 -0.030376947 ;
	setAttr ".tk[7]" -type "float3" -4.1078252e-015 4.1078252e-015 0 ;
	setAttr ".tk[16]" -type "float3" 0 4.1078252e-015 0 ;
	setAttr ".tk[50]" -type "float3" -4.1078252e-015 4.1078252e-015 0.035463795 ;
	setAttr ".tk[51]" -type "float3" 6.1211365e-023 4.1078252e-015 0.05015333 ;
	setAttr ".tk[52]" -type "float3" 4.1078252e-015 4.1078252e-015 0.035463795 ;
	setAttr ".tk[53]" -type "float3" 4.1078252e-015 4.1078252e-015 2.1439406e-010 ;
	setAttr ".tk[54]" -type "float3" 4.1078252e-015 4.1078252e-015 -0.035463795 ;
	setAttr ".tk[55]" -type "float3" 6.1211365e-023 4.1078252e-015 -0.05015333 ;
	setAttr ".tk[56]" -type "float3" -4.1078252e-015 4.1078252e-015 -0.035463776 ;
	setAttr ".tk[57]" -type "float3" -4.1078252e-015 4.1078252e-015 2.1439406e-010 ;
createNode polyMapCut -n "polyMapCut1";
	rename -uid "C87AF6AE-4339-AAE1-698F-3ABD7C08FE67";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[0:7]";
createNode polyMapCut -n "polyMapCut2";
	rename -uid "404249DB-4546-6905-1651-D98195FA426E";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 40 "e[16]" "e[18]" "e[20]" "e[22]" "e[24]" "e[26]" "e[28]" "e[30]" "e[32]" "e[34]" "e[36]" "e[38]" "e[40]" "e[42]" "e[44]" "e[46]" "e[56]" "e[58]" "e[60]" "e[62]" "e[72]" "e[74]" "e[76]" "e[78]" "e[88]" "e[90]" "e[92]" "e[94]" "e[104]" "e[106]" "e[108]" "e[110]" "e[120]" "e[122]" "e[124]" "e[126]" "e[136]" "e[138]" "e[140]" "e[142]";
createNode Unfold3DUnfold -n "Unfold3DUnfold1";
	rename -uid "C347AC9E-4467-85F9-EB8A-7CA54431F1C1";
	setAttr ".uvl" -type "Int32Array" 128 0 1 2 3 4 5
		 6 7 8 9 10 11 12 13 14 15 16 17
		 18 19 20 21 22 23 24 25 26 27 28 29
		 30 31 32 33 34 35 36 37 38 39 40 41
		 42 43 44 45 46 47 48 49 50 51 52 53
		 54 55 56 57 58 59 60 61 62 63 64 65
		 66 67 68 69 70 71 72 73 74 75 76 77
		 78 79 80 81 82 83 84 85 86 87 88 89
		 90 91 92 93 94 95 96 97 98 99 100 101
		 102 103 104 105 106 107 108 109 110 111 112 113
		 114 115 116 117 118 119 120 121 122 123 124 125
		 126 127 ;
	setAttr ".mdp" -type "string" "|pCylinder1|pCylinderShape1";
	setAttr ".bi" no;
	setAttr ".usn" -type "string" "map1";
	setAttr ".miee" yes;
	setAttr ".mue" -type "floatArray" 128 0.56458741 0.46718654 0.40900913 0.42352644
		 0.47976473 0.52087492 0.5915063 0.59555674 0.55518991 0.5371334 0.502545 0.48185539
		 0.35625103 0.34187898 0.56845498 0.54258806 0.4995034 0.53757805 0.57706821 0.39158872
		 0.40486994 0.47375631 0.49601713 0.53178209 0.5514583 0.54942876 0.35810262 0.54740387
		 0.54656619 0.53333396 0.48990762 0.47608313 0.3874414 0.37479475 0.35282046 0.54640234
		 0.5475589 0.53478938 0.49113593 0.47756624 0.38196146 0.36736509 0.67274702 0.54525197
		 0.61298341 0.4497807 0.52828664 0.52155381 0.59494048 0.57210767 0.70560187 0.66324151
		 0.34476608 0.4291386 0.43206185 0.51206166 0.48248222 0.59109855 0.39136258 0.55904055
		 0.57828885 0.52700633 0.51999795 0.46333149 0.49980018 0.46231416 0.33455336 0.544245
		 0.55461216 0.53557873 0.50056815 0.47944492 0.37304386 0.35286373 0.59488326 0.69632179
		 0.58719224 0.51119739 0.50244451 0.4260186 0.77456754 0.68653363 0.45945019 0.46176198
		 0.51831657 0.51881808 0.51770067 0.51514775 0.57152396 0.31817979 0.40832394 0.45284611
		 0.47554183 0.51257008 0.48317111 0.51996481 0.62977868 0.36219347 0.5964269 0.46772861
		 0.51574266 0.44737321 0.7585814 0.37173498 0.58771217 0.45765987 0.46442124 0.52211809
		 0.52882868 0.56413627 0.46311045 0.52119446 0.52952641 0.56502861 0.53873461 0.48620957
		 0.30638772 0.48426774 0.49771926 0.50763243 0.49857622 0.57025933 0.43785056 0.44248632
		 0.35102385 0.41074929 0.61861187 0.53536987 ;
	setAttr ".mve" -type "floatArray" 128 0.15015005 0.15844467 0.20012867 0.2487753
		 0.30056691 0.31487909 0.26974687 0.23150063 0.64532322 0.64296997 0.62601638 0.62770373
		 0.67423719 0.64982063 0.66230333 0.65476006 0.2348097 0.68369478 0.61604089 0.60055989
		 0.62902027 0.5917567 0.59035528 0.59831131 0.60281432 0.60742277 0.60575575 0.62117827
		 0.61320162 0.61319947 0.60034984 0.60221255 0.63294405 0.61669731 0.61169022 0.62918985
		 0.61843842 0.62038469 0.60548842 0.60825342 0.6391688 0.62535274 0.48305094 0.46073577
		 0.55271494 0.48010325 0.46833217 0.46517846 0.46715125 0.46093762 0.35150218 0.35014135
		 0.3987633 0.38512787 0.36090672 0.36167717 0.31364664 0.3362186 0.48982897 0.54438162
		 0.54637998 0.53698224 0.53935909 0.54374647 0.60801083 0.53606981 0.61841762 0.64166284
		 0.63119459 0.63281161 0.61422324 0.61782277 0.6600219 0.63820112 0.31497577 0.32743707
		 0.32949743 0.34203133 0.34986302 0.36879984 0.41161358 0.33030006 0.62278926 0.63434547
		 0.63428491 0.64801276 0.63841742 0.65350014 0.64708853 0.63215631 0.56354469 0.60062766
		 0.55470258 0.60729569 0.5428915 0.60848713 0.56532532 0.58556283 0.34971002 0.45028043
		 0.36462814 0.47876507 0.44227266 0.50114727 0.25119293 0.38265583 0.61178344 0.62236542
		 0.62737751 0.63212657 0.60684896 0.61734354 0.6207872 0.62532711 0.67312664 0.64990407
		 0.68061435 0.26673856 0.20851059 0.18716575 0.29429299 0.31478962 0.3402445 0.2851986
		 0.38040861 0.17339717 0.23854756 0.18019778 ;
	setAttr ".pack" no;
createNode polyTweakUV -n "polyTweakUV1";
	rename -uid "EE779A05-4CD5-375D-E7BF-1494A8AB34EC";
	setAttr ".uopa" yes;
	setAttr -s 124 ".uvtk";
	setAttr ".uvtk[0]" -type "float2" -0.15179196 0.23612083 ;
	setAttr ".uvtk[3]" -type "float2" -0.11806042 0.019275166 ;
	setAttr ".uvtk[4]" -type "float2" -0.11806042 0.019275166 ;
	setAttr ".uvtk[5]" -type "float2" 0.067463063 0.11565101 ;
	setAttr ".uvtk[6]" -type "float2" 0.067463063 0.11565101 ;
	setAttr ".uvtk[7]" -type "float2" -0.15179196 0.23612085 ;
	setAttr ".uvtk[8]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[9]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[10]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[11]" -type "float2" -0.024093965 0.30358395 ;
	setAttr ".uvtk[12]" -type "float2" 0.41200677 -0.15179195 ;
	setAttr ".uvtk[13]" -type "float2" 0.41200677 -0.15179195 ;
	setAttr ".uvtk[14]" -type "float2" 0.29635572 0.38791275 ;
	setAttr ".uvtk[15]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[16]" -type "float2" -0.15179196 0.23612085 ;
	setAttr ".uvtk[17]" -type "float2" 0.29635572 0.38791287 ;
	setAttr ".uvtk[18]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[19]" -type "float2" 0.41200677 -0.15179197 ;
	setAttr ".uvtk[20]" -type "float2" 0.41200677 -0.15179197 ;
	setAttr ".uvtk[21]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[22]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[23]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[24]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[25]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[26]" -type "float2" 0.41200674 -0.15179197 ;
	setAttr ".uvtk[27]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[28]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[29]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[30]" -type "float2" -0.024093965 0.30358395 ;
	setAttr ".uvtk[31]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[32]" -type "float2" 0.4120068 -0.15179197 ;
	setAttr ".uvtk[33]" -type "float2" 0.41200677 -0.15179197 ;
	setAttr ".uvtk[34]" -type "float2" 0.41200674 -0.15179197 ;
	setAttr ".uvtk[35]" -type "float2" 0.29635572 0.38791275 ;
	setAttr ".uvtk[36]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[37]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[38]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[39]" -type "float2" -0.024093965 0.30358395 ;
	setAttr ".uvtk[40]" -type "float2" 0.4120068 -0.15179197 ;
	setAttr ".uvtk[41]" -type "float2" 0.41200677 -0.15179197 ;
	setAttr ".uvtk[42]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[43]" -type "float2" 0.4120068 -0.15179197 ;
	setAttr ".uvtk[44]" -type "float2" 0.4120068 -0.15179197 ;
	setAttr ".uvtk[45]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[46]" -type "float2" -0.024093935 0.30358389 ;
	setAttr ".uvtk[47]" -type "float2" -0.29394633 0.22166447 ;
	setAttr ".uvtk[48]" -type "float2" -0.29394633 0.22166447 ;
	setAttr ".uvtk[49]" -type "float2" 0.29635572 0.38791278 ;
	setAttr ".uvtk[50]" -type "float2" 0.29635572 0.38791278 ;
	setAttr ".uvtk[51]" -type "float2" 0.41200674 -0.15179196 ;
	setAttr ".uvtk[52]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[53]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[54]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[55]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[56]" -type "float2" 0.29635572 0.38791284 ;
	setAttr ".uvtk[57]" -type "float2" 0.29635572 0.38791278 ;
	setAttr ".uvtk[58]" -type "float2" 0.41200677 -0.15179197 ;
	setAttr ".uvtk[59]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[60]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[61]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[62]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[63]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[64]" -type "float2" 0.41200677 -0.15179197 ;
	setAttr ".uvtk[65]" -type "float2" 0.41200677 -0.15179197 ;
	setAttr ".uvtk[66]" -type "float2" 0.4120068 -0.15179197 ;
	setAttr ".uvtk[67]" -type "float2" 0.29635572 0.38791287 ;
	setAttr ".uvtk[68]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[69]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[70]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[71]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[72]" -type "float2" 0.41200677 -0.15179195 ;
	setAttr ".uvtk[73]" -type "float2" 0.41200674 -0.15179197 ;
	setAttr ".uvtk[74]" -type "float2" 0.29635572 0.38791278 ;
	setAttr ".uvtk[75]" -type "float2" 0.29635572 0.38791278 ;
	setAttr ".uvtk[76]" -type "float2" -0.29394633 0.22166447 ;
	setAttr ".uvtk[77]" -type "float2" -0.29394633 0.22166447 ;
	setAttr ".uvtk[78]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[79]" -type "float2" -0.024093965 0.30358395 ;
	setAttr ".uvtk[80]" -type "float2" 0.41200674 -0.15179197 ;
	setAttr ".uvtk[81]" -type "float2" 0.4120068 -0.15179196 ;
	setAttr ".uvtk[82]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[83]" -type "float2" -0.024093965 0.30358395 ;
	setAttr ".uvtk[84]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[85]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[86]" -type "float2" 0.29635572 0.38791287 ;
	setAttr ".uvtk[87]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[88]" -type "float2" 0.29635572 0.38791287 ;
	setAttr ".uvtk[89]" -type "float2" 0.41200674 -0.15179197 ;
	setAttr ".uvtk[90]" -type "float2" -0.024093965 0.30358395 ;
	setAttr ".uvtk[91]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[92]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[93]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[94]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[95]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[96]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[97]" -type "float2" 0.4120068 -0.15179197 ;
	setAttr ".uvtk[98]" -type "float2" -0.29394633 0.22166447 ;
	setAttr ".uvtk[99]" -type "float2" 0.29635572 0.38791278 ;
	setAttr ".uvtk[100]" -type "float2" -0.024093965 0.30358395 ;
	setAttr ".uvtk[101]" -type "float2" -0.29394633 0.22166447 ;
	setAttr ".uvtk[102]" -type "float2" 0.4120068 -0.15179197 ;
	setAttr ".uvtk[103]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[104]" -type "float2" 0.4120068 -0.15179196 ;
	setAttr ".uvtk[105]" -type "float2" 0.41200677 -0.15179196 ;
	setAttr ".uvtk[106]" -type "float2" -0.024093965 0.30358395 ;
	setAttr ".uvtk[107]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[108]" -type "float2" 0.29635572 0.38791287 ;
	setAttr ".uvtk[109]" -type "float2" 0.29635572 0.38791287 ;
	setAttr ".uvtk[110]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[111]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[112]" -type "float2" 0.29635572 0.38791287 ;
	setAttr ".uvtk[113]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[114]" -type "float2" -0.29394633 0.22166444 ;
	setAttr ".uvtk[115]" -type "float2" -0.024093965 0.30358395 ;
	setAttr ".uvtk[116]" -type "float2" 0.41200674 -0.15179195 ;
	setAttr ".uvtk[118]" -type "float2" -0.11806042 0.019275166 ;
	setAttr ".uvtk[119]" -type "float2" 0.067463063 0.11565103 ;
	setAttr ".uvtk[120]" -type "float2" 0.29635572 0.38791281 ;
	setAttr ".uvtk[121]" -type "float2" -0.15179196 0.23612083 ;
	setAttr ".uvtk[122]" -type "float2" -0.29394633 0.22166447 ;
	setAttr ".uvtk[123]" -type "float2" 0.067463063 0.11565101 ;
	setAttr ".uvtk[124]" -type "float2" -0.024093965 0.30358389 ;
	setAttr ".uvtk[125]" -type "float2" -0.11806042 0.019275166 ;
	setAttr ".uvtk[126]" -type "float2" 0.4120068 -0.15179196 ;
createNode polyMergeUV -n "polyMergeUV2";
	rename -uid "1F0B8C93-4E8F-E473-6631-739CC9F11864";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 7 "map[0:7]" "map[16]" "map[117:119]" "map[121]" "map[123]" "map[125]" "map[127]";
	setAttr ".d" 100;
createNode Unfold3DUnfold -n "Unfold3DUnfold2";
	rename -uid "91C2F71C-4D83-DF31-45EE-AB87D52F339C";
	setAttr ".uvl" -type "Int32Array" 16 0 1 2 3 4 5
		 6 7 16 117 118 119 121 123 125 127 ;
	setAttr ".mdp" -type "string" "|pCylinder1|pCylinderShape1";
	setAttr ".bi" no;
	setAttr ".usn" -type "string" "map1";
	setAttr ".miee" yes;
	setAttr ".mue" -type "floatArray" 121 0.40617847 0.32746696 0.28405625 0.30137575
		 0.36927992 0.44799146 0.49140215 0.26137894 0.24318707 0.47845104 0.45776144 0.7682578
		 0.75388575 0.86481071 0.83894378 0.83393377 0.87318653 0.80359548 0.81687671 0.44966236
		 0.47192317 0.23783576 0.25751197 0.84578449 0.77010936 0.8437596 0.25261986 0.23938763
		 0.46581367 0.45198917 0.79944819 0.78680152 0.76482719 0.84275806 0.25361258 0.24084306
		 0.46704197 0.45347229 0.79396826 0.77937186 0.96910274 0.95725876 1.0249902 0.42568675
		 0.50419271 0.22760749 0.30099416 0.8684634 1.0019577 1.0752482 0.32067212 0.40504465
		 0.13811553 0.21811533 0.77883792 0.88745427 0.80336934 0.85539627 0.28434253 0.23306
		 0.495904 0.43923753 0.91180694 0.87432092 0.74656016 0.84060073 0.26066583 0.2416324
		 0.4764742 0.45535097 0.78505063 0.76487046 0.89123899 0.99267751 0.29324591 0.21725106
		 0.47835055 0.40192464 1.1865742 1.0985404 0.43535623 0.43766803 0.22437024 0.22487175
		 0.8140564 0.81150347 0.86787969 0.73018652 0.38422999 0.42875215 0.1815955 0.21862376
		 0.77952683 0.81632054 0.92613441 0.77420026 0.30248058 0.76408434 0.4916487 0.15342689
		 1.1705883 0.34764102 0.99971896 0.86966664 0.44032729 0.22817177 0.8251844 0.86049199
		 0.43901649 0.22724813 0.82588214 0.86138433 0.24478829 0.46211562 0.71839446 0.3877292
		 0.79493195 0.142602 0.32620063 1.0289924 0.47408265 ;
	setAttr ".mve" -type "floatArray" 121 0.23566787 0.26170713 0.34609866 0.43940705
		 0.48697352 0.46093425 0.37654272 0.8666302 0.86463439 0.92960024 0.93128765 0.52244526
		 0.4980287 1.0502161 1.0426729 1.0716076 1.0041736 0.4487679 0.47722828 0.89534056
		 0.89393914 0.81997573 0.82447875 0.99533558 0.45396376 1.0090911 0.83486605 0.8348639
		 0.90393376 0.90579641 0.48115206 0.46490532 0.45989823 1.0171026 0.84010285 0.84204912
		 0.90907228 0.91183734 0.48737681 0.47356075 0.87096375 0.30894381 0.40092295 0.78368711
		 0.77191603 0.68684292 0.68881571 0.84885037 0.73941493 0.19834939 0.70234716 0.68871176
		 0.58257115 0.5833416 0.70155948 0.72413135 0.33803701 0.93229443 0.76804441 0.75864667
		 0.84294295 0.84733033 0.45621884 0.38427782 0.46662563 1.0295757 0.85285902 0.85447603
		 0.9178071 0.92140663 0.50822997 0.48640913 0.70288855 0.71534985 0.55116189 0.56369579
		 0.65344691 0.67238379 0.25982159 0.1785081 0.92637312 0.93792939 0.85594934 0.86967719
		 1.0263302 1.0414129 1.0350014 0.48036432 0.86712861 0.90421152 0.77636701 0.82896012
		 0.93080431 0.99639994 0.95323813 0.43377084 0.57137448 0.83819318 0.66821206 0.70042956
		 0.29048067 0.80473113 0.099400967 0.23086387 0.91536736 0.84402984 1.0152904 1.0200394
		 0.91043282 0.83900797 1.0087001 1.0132399 0.89479107 0.95348799 0.52882242 0.3613207
		 0.6822058 0.56221956 0.68426144 0.085453808 0.28323433 ;
	setAttr ".pack" no;
createNode polyMapCut -n "polyMapCut3";
	rename -uid "5B66539B-4E62-C501-836F-ABA5A0227631";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 1 "e[48:55]";
createNode polyTweakUV -n "polyTweakUV2";
	rename -uid "E9C8762F-4455-1F16-3EC5-BEA4931A926E";
	setAttr ".uopa" yes;
	setAttr -s 65 ".uvtk";
	setAttr ".uvtk[7]" -type "float2" 0.39587885 -0.29690912 ;
	setAttr ".uvtk[8]" -type "float2" 0.39587891 -0.29690912 ;
	setAttr ".uvtk[9]" -type "float2" 0.22621647 -0.36194643 ;
	setAttr ".uvtk[10]" -type "float2" 0.22621647 -0.36194643 ;
	setAttr ".uvtk[11]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[12]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[13]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[14]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[15]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[24]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[25]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[26]" -type "float2" 0.39587891 -0.29690912 ;
	setAttr ".uvtk[27]" -type "float2" 0.39587891 -0.29690912 ;
	setAttr ".uvtk[28]" -type "float2" 0.22621647 -0.36194643 ;
	setAttr ".uvtk[29]" -type "float2" 0.2262165 -0.36194643 ;
	setAttr ".uvtk[30]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[31]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[32]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[33]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[34]" -type "float2" 0.39587885 -0.29690912 ;
	setAttr ".uvtk[35]" -type "float2" 0.39587891 -0.29690912 ;
	setAttr ".uvtk[36]" -type "float2" 0.22621652 -0.36194643 ;
	setAttr ".uvtk[37]" -type "float2" 0.22621647 -0.36194643 ;
	setAttr ".uvtk[38]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[39]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[64]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[65]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[66]" -type "float2" 0.39587885 -0.29690912 ;
	setAttr ".uvtk[67]" -type "float2" 0.39587885 -0.29690912 ;
	setAttr ".uvtk[68]" -type "float2" 0.22621647 -0.36194643 ;
	setAttr ".uvtk[69]" -type "float2" 0.22621652 -0.36194643 ;
	setAttr ".uvtk[70]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[71]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[80]" -type "float2" 0.22621652 -0.36194643 ;
	setAttr ".uvtk[81]" -type "float2" 0.22621647 -0.36194643 ;
	setAttr ".uvtk[82]" -type "float2" 0.39587888 -0.29690912 ;
	setAttr ".uvtk[83]" -type "float2" 0.39587888 -0.29690912 ;
	setAttr ".uvtk[84]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[85]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[86]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[87]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[89]" -type "float2" 0.22621652 -0.36194643 ;
	setAttr ".uvtk[91]" -type "float2" 0.39587888 -0.29690912 ;
	setAttr ".uvtk[93]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[95]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[104]" -type "float2" 0.22621652 -0.36194643 ;
	setAttr ".uvtk[105]" -type "float2" 0.39587885 -0.29690912 ;
	setAttr ".uvtk[106]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[107]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[108]" -type "float2" 0.22621647 -0.36194643 ;
	setAttr ".uvtk[109]" -type "float2" 0.39587885 -0.29690912 ;
	setAttr ".uvtk[110]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[111]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[112]" -type "float2" 0.39587891 -0.29690912 ;
	setAttr ".uvtk[113]" -type "float2" 0.22621652 -0.36194643 ;
	setAttr ".uvtk[114]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[121]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[122]" -type "float2" -0.19511174 -0.54291958 ;
	setAttr ".uvtk[124]" -type "float2" 0.39587891 -0.29690912 ;
	setAttr ".uvtk[126]" -type "float2" 0.39587891 -0.29690912 ;
	setAttr ".uvtk[127]" -type "float2" 0.22621647 -0.36194643 ;
	setAttr ".uvtk[129]" -type "float2" 0.22621647 -0.36194643 ;
	setAttr ".uvtk[130]" -type "float2" -0.14704071 0.0028277063 ;
	setAttr ".uvtk[132]" -type "float2" -0.14704071 0.0028277063 ;
createNode polyMergeUV -n "polyMergeUV3";
	rename -uid "DD196679-4AB2-2811-10AF-7E97B678E5B9";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 14 "map[7:15]" "map[24:39]" "map[64:71]" "map[80:87]" "map[89]" "map[91]" "map[93]" "map[95]" "map[104:114]" "map[121:122]" "map[124]" "map[126:127]" "map[129:130]" "map[132]";
	setAttr ".d" 100;
createNode Unfold3DUnfold -n "Unfold3DUnfold3";
	rename -uid "57A0004D-4037-2D30-6733-749F3D120765";
	setAttr ".uvl" -type "Int32Array" 64 7 8 9 10 11 12
		 13 14 15 24 25 26 27 28 29 30 31 32
		 33 34 35 36 37 38 39 64 65 66 67 68
		 69 70 71 80 81 82 83 84 85 86 87 89
		 91 93 95 104 105 106 107 108 109 110 111 112
		 113 114 121 122 124 126 127 129 130 132 ;
	setAttr ".mdp" -type "string" "|pCylinder1|pCylinderShape1";
	setAttr ".bi" no;
	setAttr ".usn" -type "string" "map1";
	setAttr ".miee" yes;
	setAttr ".mue" -type "floatArray" 110 0.40617847 0.32746696 0.28405625 0.30137575
		 0.36927992 0.44799146 0.49140215 0.64807534 0.64301187 0.63893837 0.63970143 0.63473129
		 0.62755424 0.64596045 0.87338692 0.80424523 0.81687671 0.44966236 0.47192317 0.23783576
		 0.25751197 0.84578449 0.65187126 0.64864784 0.63948911 0.63877881 0.64629722 0.65643734
		 0.66099036 0.65717441 0.64668459 0.64126432 0.65156502 0.64626461 0.96901381 0.95746022
		 1.0250682 0.42588961 0.50394428 0.2280672 0.30099416 0.8684634 1.0019577 1.0744458
		 0.32067212 0.40459222 0.13811553 0.21779235 0.77883792 0.88745427 0.80336934 0.85539627
		 0.28434253 0.23306 0.495904 0.43923753 0.91180694 0.87432092 0.6447894 0.63751125
		 0.64470488 0.61782974 0.89123899 0.99267751 0.29269794 0.21684517 0.47763011 0.40128663
		 1.1861252 1.0976907 0.63870633 0.6442948 0.64401132 0.62393564 0.64084792 0.38422999
		 0.1815955 0.77952683 0.92613441 0.65261716 0.30199996 0.76408404 0.49095643 0.15397146
		 1.1702278 0.34822696 0.99792087 0.86890513 0.65269381 0.64536303 0.64301854 0.64507389
		 0.64166772 0.3877292 0.79493195 0.14372312 0.32620063 1.0289924 0.47408265 0.65067273
		 0.77420026 0.63729984 0.81632054 0.63371468 0.64379036 0.21791986 0.6637966 0.67847198
		 0.42818776 0.67906076 ;
	setAttr ".mve" -type "floatArray" 110 0.23566787 0.26170713 0.34609866 0.43940705
		 0.48697352 0.46093425 0.37654272 0.52317339 0.5406794 0.527399 0.51486331 0.50321525
		 0.49419725 0.49787417 1.0040686 0.44842026 0.47722828 0.89534056 0.89393914 0.81997573
		 0.82447875 0.99533558 0.46581069 0.46617156 0.50634789 0.52435237 0.52827263 0.51910698
		 0.500884 0.48139846 0.48048913 0.52782655 0.51673657 0.48552197 0.87121451 0.3082577
		 0.39955083 0.78326845 0.77145791 0.68659621 0.68881571 0.84885037 0.73941493 0.19646044
		 0.70234716 0.68810093 0.58257115 0.58289635 0.70155948 0.72413135 0.33803701 0.93229443
		 0.76804441 0.75864667 0.84294295 0.84733033 0.45621884 0.38427782 0.49425736 0.55756688
		 0.5143677 0.48923683 0.70288855 0.71534985 0.55042732 0.56325054 0.65273917 0.67182297
		 0.25758806 0.17659481 0.49956831 0.52913004 0.53177017 0.47699618 0.49926171 0.86712861
		 0.77636701 0.93080431 0.95323813 0.4489263 0.57059771 0.83809453 0.66745645 0.70071626
		 0.28820306 0.80468637 0.098334357 0.23113465 0.50126266 0.52643007 0.51958603 0.47227815
		 0.50643748 0.3613207 0.6822058 0.56188524 0.68426144 0.085453808 0.28323433 0.452416
		 0.43377084 0.49052501 0.99639994 0.52306664 0.53528345 0.82893133 0.52699435 0.50153655
		 0.90453094 0.47199515 ;
	setAttr ".pack" no;
createNode polyMapCut -n "polyMapCut4";
	rename -uid "BD0CB16B-4285-A4F4-10AA-C9A7F39410B6";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 5 "e[36]" "e[44]" "e[60]" "e[76]" "e[140]";
createNode Unfold3DUnfold -n "Unfold3DUnfold4";
	rename -uid "CF187BAE-4752-2698-64C1-CC9B64498B31";
	setAttr ".uvl" -type "Int32Array" 46 7 8 9 10 11 12
		 13 22 23 24 25 26 27 28 29 30 31 32
		 33 58 59 60 61 70 71 72 73 74 79 88
		 89 90 91 92 99 101 103 104 106 107 109 110
		 111 112 113 114 ;
	setAttr ".mdp" -type "string" "|pCylinder1|pCylinderShape1";
	setAttr ".bi" no;
	setAttr ".usn" -type "string" "map1";
	setAttr ".miee" yes;
	setAttr ".mue" -type "floatArray" 115 0.40617847 0.32746696 0.28405625 0.30137575
		 0.36927992 0.44799146 0.49140215 0.62569451 0.61833757 0.68160707 0.6727733 0.66465837
		 0.65629739 0.6361019 0.87338692 0.80424523 0.81687671 0.44966236 0.47192317 0.23783576
		 0.25751197 0.84578449 0.64468992 0.627101 0.61170107 0.60098672 0.69880903 0.68930781
		 0.67609489 0.66106904 0.62885803 0.60551476 0.68586421 0.66061658 0.96901381 0.95746022
		 1.0250682 0.42588961 0.50394428 0.2280672 0.30099416 0.8684634 1.0019577 1.0744458
		 0.32067212 0.40459222 0.13811553 0.21779235 0.77883792 0.88745427 0.80336934 0.85539627
		 0.28434253 0.23306 0.495904 0.43923753 0.91180694 0.87432092 0.63157946 0.61040407
		 0.68034673 0.65734917 0.89123899 0.99267751 0.29269794 0.21684517 0.47763011 0.40128663
		 1.1861252 1.0976907 0.66861129 0.69246465 0.61841601 0.64594787 0.64676917 0.38422999
		 0.1815955 0.77952683 0.92613441 0.64644295 0.30199996 0.76408404 0.49095643 0.15397146
		 1.1702278 0.34822696 0.99792087 0.86890513 0.67413265 0.69539779 0.61512864 0.64512616
		 0.64789653 0.3877292 0.79493195 0.14372312 0.32620063 1.0289924 0.47408265 0.62293845
		 0.77420026 0.60314089 0.81632054 0.58991456 0.71030492 0.21791986 0.69680667 0.68023992
		 0.42818776 0.66529948 0.60318089 0.61401093 0.59873599 0.59561712 0.58370799 ;
	setAttr ".mve" -type "floatArray" 115 0.23566787 0.26170713 0.34609866 0.43940705
		 0.48697352 0.46093425 0.37654272 0.51894611 0.52934325 0.54279083 0.52534682 0.51393551
		 0.50853878 0.51191044 1.0040686 0.44842026 0.47722828 0.89534056 0.89393914 0.81997573
		 0.82447875 0.99533558 0.48441637 0.48890713 0.4979018 0.50876981 0.51425117 0.50282097
		 0.49209201 0.48554876 0.49372438 0.51305598 0.50782663 0.49092937 0.87121451 0.3082577
		 0.39955083 0.78326845 0.77145791 0.68659621 0.68881571 0.84885037 0.73941493 0.19646044
		 0.70234716 0.68810093 0.58257115 0.58289635 0.70155948 0.72413135 0.33803701 0.93229443
		 0.76804441 0.75864667 0.84294295 0.84733033 0.45621884 0.38427782 0.50185722 0.52318394
		 0.51670074 0.49880111 0.70288855 0.71534985 0.55042732 0.56325054 0.65273917 0.67182297
		 0.25758806 0.17659481 0.50438464 0.53415513 0.51173723 0.49756834 0.50798923 0.86712861
		 0.77636701 0.93080431 0.95323813 0.4723717 0.57059771 0.83809453 0.66745645 0.70071626
		 0.28820306 0.80468637 0.098334357 0.23113465 0.49729994 0.51912361 0.50232273 0.48954281
		 0.53629953 0.3613207 0.6822058 0.56188524 0.68426144 0.085453808 0.28323433 0.4794156
		 0.43377084 0.4901306 0.99639994 0.50088483 0.51326674 0.82893133 0.49562871 0.48143876
		 0.90453094 0.47330803 0.53511465 0.54445493 0.52309805 0.51978493 0.51880878 ;
	setAttr ".pack" no;
createNode polyTweakUV -n "polyTweakUV3";
	rename -uid "5270201E-4D91-54B7-F921-009F9FAF060D";
	setAttr ".uopa" yes;
	setAttr -s 115 ".uvtk[0:114]" -type "float2" -0.34711921 -0.22052749 -0.29241949
		 -0.23589101 -0.26006064 -0.2923063 -0.26899803 -0.35672608 -0.31399626 -0.39141408
		 -0.36869597 -0.37605056 -0.40105483 -0.31963524 -0.074384168 -0.29600638 -0.095315441
		 -0.26642546 0.084692776 -0.22816572 0.059559762 -0.27779573 0.036472023 -0.31026208
		 0.01268415 -0.3256163 -0.044773996 -0.31602356 -0.088824257 -0.012619004 -0.5631339
		 0.54801017 -0.52352655 0.51920217 -0.20855111 0.10108989 -0.17857298 0.10249132 0.0032755435
		 0.17645474 0.035838246 0.17195173 -0.11346073 -0.0038859695 -0.020340476 -0.39424697
		 -0.07038261 -0.38147032 -0.11419691 -0.35587958 -0.14468041 -0.32495892 0.13363393
		 -0.3093639 0.10660207 -0.34188402 0.069010139 -0.37240899 0.026259942 -0.39102522
		 -0.065383568 -0.36776477 -0.13179764 -0.31276435 0.096804857 -0.32764247 0.024972601
		 -0.37571687 -0.014909881 -0.21123174 -0.70739204 0.35670596 -0.5621767 0.26541281
		 -0.17582136 -0.11830483 -0.041052818 -0.10649429 0.022001103 -0.021632582 0.1618973
		 -0.02385208 -0.12718262 -0.18886763 -0.021986563 -0.35383731 -0.82554847 0.19409825
		 -0.30148569 -0.3117885 -0.1556949 -0.29754227 -0.11892918 -0.19201247 0.031104952
		 -0.19233768 -0.26843902 -0.31598169 -0.14734448 -0.33855355 -0.71174026 0.50371063
		 -0.11593582 -0.095527619 0.11971724 0.073703237 0.015187949 0.083100982 -0.091844231
		 -0.0011952966 -0.19098961 -0.0055826753 -0.50774705 0.38552898 -0.626073 0.45746976
		 -0.057640851 -0.34462601 -0.11788704 -0.28394932 0.08110714 -0.30239463 0.01567666
		 -0.35332096 -0.15043949 -0.36184332 -0.039364636 -0.37430462 0.16940236 -0.20440124
		 0.03274183 -0.21722446 -0.015529811 -0.30671307 -0.15169957 -0.32579687 -0.72402489
		 0.088438042 -0.84810364 0.16943131 0.047718406 -0.33743528 0.1155836 -0.25273514
		 -0.095092282 -0.31651637 -0.016761262 -0.35682827 -0.014424521 -0.32717979 -0.29260099
		 -0.025380954 -0.089966536 0.06538064 -0.19668533 -0.094037503 -0.030861987 -0.11647147
		 -0.015352625 -0.42851529 0.18675859 -0.18003903 -0.24014781 -0.17811179 -0.0021978766
		 -0.27689779 -0.12124768 -0.035752624 -0.6814692 0.10235544 -0.31550312 -0.13972275
		 -0.97873467 0.29222429 -0.83618128 0.43382895 0.063427269 -0.35759193 0.12392873
		 -0.29550135 -0.10444517 -0.34330159 -0.01909909 -0.37966174 -0.011217255 -0.24663413
		 -0.33055773 -0.30597079 -0.25948399 -0.34116057 -0.099487737 -0.21585916 -0.28196529
		 -0.33823535 -0.98475695 0.26057228 -0.39211744 -0.2552155 -0.082225516 -0.40847471
		 -0.58837748 0.56265962 -0.1385515 -0.37798941 -0.13928521 -0.0049503297 -0.17618176
		 -0.34739256 0.16634108 -0.31216475 -0.032097012 0.16749914 0.12793715 -0.36234671
		 0.080803037 -0.40271857 -0.24236497 0.091899522 0.038296044 -0.42585132 -0.13843778
		 -0.2500053 -0.10762523 -0.2234312 -0.15108392 -0.28419366 -0.15995738 -0.29361984
		 -0.19384006 -0.29639712;
createNode script -n "uiConfigurationScriptNode";
	rename -uid "B59D4CE1-4598-0FF1-B2DB-D98865B2A8F1";
	setAttr ".b" -type "string" (
		"// Maya Mel UI Configuration File.\n//\n//  This script is machine generated.  Edit at your own risk.\n//\n//\n\nglobal string $gMainPane;\nif (`paneLayout -exists $gMainPane`) {\n\n\tglobal int $gUseScenePanelConfig;\n\tint    $useSceneConfig = $gUseScenePanelConfig;\n\tint    $menusOkayInPanels = `optionVar -q allowMenusInPanels`;\tint    $nVisPanes = `paneLayout -q -nvp $gMainPane`;\n\tint    $nPanes = 0;\n\tstring $editorName;\n\tstring $panelName;\n\tstring $itemFilterName;\n\tstring $panelConfig;\n\n\t//\n\t//  get current state of the UI\n\t//\n\tsceneUIReplacement -update $gMainPane;\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Top View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Top View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            modelEditor -e \n                -camera \"top\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"smoothShaded\" \n"
		+ "                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -holdOuts 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 0\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -depthOfFieldPreview 1\n                -maxConstantTransparency 1\n"
		+ "                -rendererName \"vp2Renderer\" \n                -objectFilterShowInHUD 1\n                -isFiltered 0\n                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 1\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n"
		+ "                -hulls 1\n                -grid 1\n                -imagePlane 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -particleInstancers 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -pluginShapes 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -clipGhosts 1\n                -greasePencils 1\n                -shadows 0\n                -captureSequenceNumber -1\n                -width 1\n                -height 1\n                -sceneRenderFilter 0\n                $editorName;\n            modelEditor -e -viewSelected 0 $editorName;\n            modelEditor -e \n"
		+ "                -pluginObjects \"gpuCacheDisplayFilter\" 1 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Top View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"top\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n"
		+ "            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n"
		+ "            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 1\n            -height 1\n            -sceneRenderFilter 0\n            $editorName;\n"
		+ "        modelEditor -e -viewSelected 0 $editorName;\n        modelEditor -e \n            -pluginObjects \"gpuCacheDisplayFilter\" 1 \n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Side View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Side View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            modelEditor -e \n                -camera \"side\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"smoothShaded\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -holdOuts 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 0\n                -backfaceCulling 0\n"
		+ "                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -depthOfFieldPreview 1\n                -maxConstantTransparency 1\n                -rendererName \"vp2Renderer\" \n                -objectFilterShowInHUD 1\n                -isFiltered 0\n                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n"
		+ "                -lowQualityLighting 0\n                -maximumNumHardwareLights 1\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -imagePlane 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -particleInstancers 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n"
		+ "                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -pluginShapes 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -clipGhosts 1\n                -greasePencils 1\n                -shadows 0\n                -captureSequenceNumber -1\n                -width 1\n                -height 1\n                -sceneRenderFilter 0\n                $editorName;\n            modelEditor -e -viewSelected 0 $editorName;\n            modelEditor -e \n                -pluginObjects \"gpuCacheDisplayFilter\" 1 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Side View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"side\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n"
		+ "            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n"
		+ "            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n"
		+ "            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 1\n            -height 1\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n        modelEditor -e \n            -pluginObjects \"gpuCacheDisplayFilter\" 1 \n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Front View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Front View\")) -mbv $menusOkayInPanels `;\n"
		+ "\t\t\t$editorName = $panelName;\n            modelEditor -e \n                -camera \"front\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"smoothShaded\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n                -holdOuts 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 0\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n"
		+ "                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -depthOfFieldPreview 1\n                -maxConstantTransparency 1\n                -rendererName \"vp2Renderer\" \n                -objectFilterShowInHUD 1\n                -isFiltered 0\n                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 1\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n"
		+ "                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -imagePlane 1\n                -joints 1\n                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -particleInstancers 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -pluginShapes 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -clipGhosts 1\n                -greasePencils 1\n                -shadows 0\n                -captureSequenceNumber -1\n"
		+ "                -width 1\n                -height 1\n                -sceneRenderFilter 0\n                $editorName;\n            modelEditor -e -viewSelected 0 $editorName;\n            modelEditor -e \n                -pluginObjects \"gpuCacheDisplayFilter\" 1 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Front View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"front\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n"
		+ "            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n"
		+ "            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n"
		+ "            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 1\n            -height 1\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n        modelEditor -e \n            -pluginObjects \"gpuCacheDisplayFilter\" 1 \n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Persp View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `modelPanel -unParent -l (localizedPanelLabel(\"Persp View\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            modelEditor -e \n                -camera \"persp\" \n                -useInteractiveMode 0\n                -displayLights \"default\" \n                -displayAppearance \"smoothShaded\" \n                -activeOnly 0\n                -ignorePanZoom 0\n                -wireframeOnShaded 0\n                -headsUpDisplay 1\n"
		+ "                -holdOuts 1\n                -selectionHiliteDisplay 1\n                -useDefaultMaterial 0\n                -bufferMode \"double\" \n                -twoSidedLighting 0\n                -backfaceCulling 0\n                -xray 0\n                -jointXray 0\n                -activeComponentsXray 0\n                -displayTextures 0\n                -smoothWireframe 0\n                -lineWidth 1\n                -textureAnisotropic 0\n                -textureHilight 1\n                -textureSampling 2\n                -textureDisplay \"modulate\" \n                -textureMaxSize 16384\n                -fogging 0\n                -fogSource \"fragment\" \n                -fogMode \"linear\" \n                -fogStart 0\n                -fogEnd 100\n                -fogDensity 0.1\n                -fogColor 0.5 0.5 0.5 1 \n                -depthOfFieldPreview 1\n                -maxConstantTransparency 1\n                -rendererName \"vp2Renderer\" \n                -objectFilterShowInHUD 1\n                -isFiltered 0\n"
		+ "                -colorResolution 256 256 \n                -bumpResolution 512 512 \n                -textureCompression 0\n                -transparencyAlgorithm \"frontAndBackCull\" \n                -transpInShadows 0\n                -cullingOverride \"none\" \n                -lowQualityLighting 0\n                -maximumNumHardwareLights 1\n                -occlusionCulling 0\n                -shadingModel 0\n                -useBaseRenderer 0\n                -useReducedRenderer 0\n                -smallObjectCulling 0\n                -smallObjectThreshold -1 \n                -interactiveDisableShadows 0\n                -interactiveBackFaceCull 0\n                -sortTransparent 1\n                -nurbsCurves 1\n                -nurbsSurfaces 1\n                -polymeshes 1\n                -subdivSurfaces 1\n                -planes 1\n                -lights 1\n                -cameras 1\n                -controlVertices 1\n                -hulls 1\n                -grid 1\n                -imagePlane 1\n                -joints 1\n"
		+ "                -ikHandles 1\n                -deformers 1\n                -dynamics 1\n                -particleInstancers 1\n                -fluids 1\n                -hairSystems 1\n                -follicles 1\n                -nCloths 1\n                -nParticles 1\n                -nRigids 1\n                -dynamicConstraints 1\n                -locators 1\n                -manipulators 1\n                -pluginShapes 1\n                -dimensions 1\n                -handles 1\n                -pivots 1\n                -textures 1\n                -strokes 1\n                -motionTrails 1\n                -clipGhosts 1\n                -greasePencils 1\n                -shadows 0\n                -captureSequenceNumber -1\n                -width 1379\n                -height 785\n                -sceneRenderFilter 0\n                $editorName;\n            modelEditor -e -viewSelected 0 $editorName;\n            modelEditor -e \n                -pluginObjects \"gpuCacheDisplayFilter\" 1 \n                $editorName;\n\t\t}\n\t} else {\n"
		+ "\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Persp View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"persp\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 16384\n            -fogging 0\n            -fogSource \"fragment\" \n"
		+ "            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n"
		+ "            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 1379\n            -height 785\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n        modelEditor -e \n            -pluginObjects \"gpuCacheDisplayFilter\" 1 \n"
		+ "            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"outlinerPanel\" (localizedPanelLabel(\"Outliner\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `outlinerPanel -unParent -l (localizedPanelLabel(\"Outliner\")) -mbv $menusOkayInPanels `;\n\t\t\t$editorName = $panelName;\n            outlinerEditor -e \n                -docTag \"isolOutln_fromSeln\" \n                -showShapes 0\n                -showReferenceNodes 1\n                -showReferenceMembers 1\n                -showAttributes 0\n                -showConnected 0\n                -showAnimCurvesOnly 0\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n                -showDagOnly 1\n                -showAssets 1\n                -showContainedOnly 1\n                -showPublishedAsConnected 0\n                -showContainerContents 1\n                -ignoreDagHierarchy 0\n"
		+ "                -expandConnections 0\n                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 1\n                -showLeafs 1\n                -showNumericAttrsOnly 0\n                -highlightActive 1\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"defaultSetFilter\" \n                -showSetMembers 1\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n"
		+ "                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 0\n                -ignoreHiddenAttribute 0\n                -ignoreOutlinerColor 0\n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\toutlinerPanel -edit -l (localizedPanelLabel(\"Outliner\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        outlinerEditor -e \n            -docTag \"isolOutln_fromSeln\" \n            -showShapes 0\n            -showReferenceNodes 1\n            -showReferenceMembers 1\n            -showAttributes 0\n            -showConnected 0\n            -showAnimCurvesOnly 0\n            -showMuteInfo 0\n            -organizeByLayer 1\n            -showAnimLayerWeight 1\n            -autoExpandLayers 1\n            -autoExpand 0\n            -showDagOnly 1\n            -showAssets 1\n            -showContainedOnly 1\n            -showPublishedAsConnected 0\n            -showContainerContents 1\n            -ignoreDagHierarchy 0\n"
		+ "            -expandConnections 0\n            -showUpstreamCurves 1\n            -showUnitlessCurves 1\n            -showCompounds 1\n            -showLeafs 1\n            -showNumericAttrsOnly 0\n            -highlightActive 1\n            -autoSelectNewObjects 0\n            -doNotSelectNewObjects 0\n            -dropIsParent 1\n            -transmitFilters 0\n            -setFilter \"defaultSetFilter\" \n            -showSetMembers 1\n            -allowMultiSelection 1\n            -alwaysToggleSelect 0\n            -directSelect 0\n            -displayMode \"DAG\" \n            -expandObjects 0\n            -setsIgnoreFilters 1\n            -containersIgnoreFilters 0\n            -editAttrName 0\n            -showAttrValues 0\n            -highlightSecondary 0\n            -showUVAttrsOnly 0\n            -showTextureNodesOnly 0\n            -attrAlphaOrder \"default\" \n            -animLayerFilterOptions \"allAffecting\" \n            -sortOrder \"none\" \n            -longNames 0\n            -niceNames 1\n            -showNamespace 1\n            -showPinIcons 0\n"
		+ "            -mapMotionTrails 0\n            -ignoreHiddenAttribute 0\n            -ignoreOutlinerColor 0\n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"graphEditor\" (localizedPanelLabel(\"Graph Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"graphEditor\" -l (localizedPanelLabel(\"Graph Editor\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showReferenceNodes 0\n                -showReferenceMembers 0\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 1\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n"
		+ "                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 0\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 1\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 1\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n"
		+ "                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 1\n                -mapMotionTrails 1\n                -ignoreHiddenAttribute 0\n                -ignoreOutlinerColor 0\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"GraphEd\");\n            animCurveEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 1\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -showResults \"off\" \n                -showBufferCurves \"off\" \n                -smoothness \"fine\" \n                -resultSamples 1\n                -resultScreenSamples 0\n                -resultUpdate \"delayed\" \n                -showUpstreamCurves 1\n                -stackedCurves 0\n                -stackedCurvesMin -1\n"
		+ "                -stackedCurvesMax 1\n                -stackedCurvesSpace 0.2\n                -displayNormalized 0\n                -preSelectionHighlight 0\n                -constrainDrag 0\n                -classicMode 1\n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Graph Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showReferenceNodes 0\n                -showReferenceMembers 0\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 1\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n"
		+ "                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 0\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 1\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 1\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n"
		+ "                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 1\n                -mapMotionTrails 1\n                -ignoreHiddenAttribute 0\n                -ignoreOutlinerColor 0\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"GraphEd\");\n            animCurveEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 1\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -showResults \"off\" \n                -showBufferCurves \"off\" \n                -smoothness \"fine\" \n                -resultSamples 1\n                -resultScreenSamples 0\n                -resultUpdate \"delayed\" \n                -showUpstreamCurves 1\n                -stackedCurves 0\n                -stackedCurvesMin -1\n                -stackedCurvesMax 1\n"
		+ "                -stackedCurvesSpace 0.2\n                -displayNormalized 0\n                -preSelectionHighlight 0\n                -constrainDrag 0\n                -classicMode 1\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dopeSheetPanel\" (localizedPanelLabel(\"Dope Sheet\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"dopeSheetPanel\" -l (localizedPanelLabel(\"Dope Sheet\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showReferenceNodes 0\n                -showReferenceMembers 0\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n"
		+ "                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 0\n                -showCompounds 1\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 1\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n"
		+ "                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 1\n                -ignoreHiddenAttribute 0\n                -ignoreOutlinerColor 0\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"DopeSheetEd\");\n            dopeSheetEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -outliner \"dopeSheetPanel1OutlineEd\" \n                -showSummary 1\n                -showScene 0\n                -hierarchyBelow 0\n                -showTicks 1\n                -selectionWindow 0 0 0 0 \n"
		+ "                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dope Sheet\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showReferenceNodes 0\n                -showReferenceMembers 0\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 0\n                -showCompounds 1\n                -showLeafs 1\n"
		+ "                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 1\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 1\n                -ignoreHiddenAttribute 0\n"
		+ "                -ignoreOutlinerColor 0\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"DopeSheetEd\");\n            dopeSheetEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -outliner \"dopeSheetPanel1OutlineEd\" \n                -showSummary 1\n                -showScene 0\n                -hierarchyBelow 0\n                -showTicks 1\n                -selectionWindow 0 0 0 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"clipEditorPanel\" (localizedPanelLabel(\"Trax Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"clipEditorPanel\" -l (localizedPanelLabel(\"Trax Editor\")) -mbv $menusOkayInPanels `;\n"
		+ "\t\t\t$editorName = clipEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 0 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Trax Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = clipEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n"
		+ "\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"sequenceEditorPanel\" (localizedPanelLabel(\"Camera Sequencer\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"sequenceEditorPanel\" -l (localizedPanelLabel(\"Camera Sequencer\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = sequenceEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 1 \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Camera Sequencer\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = sequenceEditorNameFromPanel($panelName);\n            clipEditor -e \n"
		+ "                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -autoFit 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -manageSequencer 1 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperGraphPanel\" (localizedPanelLabel(\"Hypergraph Hierarchy\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"hyperGraphPanel\" -l (localizedPanelLabel(\"Hypergraph Hierarchy\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"HyperGraphEd\");\n            hyperGraph -e \n                -graphLayoutStyle \"hierarchicalLayout\" \n                -orientation \"horiz\" \n                -mergeConnections 0\n                -zoom 1\n                -animateTransition 0\n                -showRelationships 1\n"
		+ "                -showShapes 0\n                -showDeformers 0\n                -showExpressions 0\n                -showConstraints 0\n                -showConnectionFromSelected 0\n                -showConnectionToSelected 0\n                -showConstraintLabels 0\n                -showUnderworld 0\n                -showInvisible 0\n                -transitionFrames 1\n                -opaqueContainers 0\n                -freeform 0\n                -imagePosition 0 0 \n                -imageScale 1\n                -imageEnabled 0\n                -graphType \"DAG\" \n                -heatMapDisplay 0\n                -updateSelection 1\n                -updateNodeAdded 1\n                -useDrawOverrideColor 0\n                -limitGraphTraversal -1\n                -range 0 0 \n                -iconSize \"smallIcons\" \n                -showCachedConnections 0\n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypergraph Hierarchy\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\t\t$editorName = ($panelName+\"HyperGraphEd\");\n            hyperGraph -e \n                -graphLayoutStyle \"hierarchicalLayout\" \n                -orientation \"horiz\" \n                -mergeConnections 0\n                -zoom 1\n                -animateTransition 0\n                -showRelationships 1\n                -showShapes 0\n                -showDeformers 0\n                -showExpressions 0\n                -showConstraints 0\n                -showConnectionFromSelected 0\n                -showConnectionToSelected 0\n                -showConstraintLabels 0\n                -showUnderworld 0\n                -showInvisible 0\n                -transitionFrames 1\n                -opaqueContainers 0\n                -freeform 0\n                -imagePosition 0 0 \n                -imageScale 1\n                -imageEnabled 0\n                -graphType \"DAG\" \n                -heatMapDisplay 0\n                -updateSelection 1\n                -updateNodeAdded 1\n                -useDrawOverrideColor 0\n                -limitGraphTraversal -1\n"
		+ "                -range 0 0 \n                -iconSize \"smallIcons\" \n                -showCachedConnections 0\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperShadePanel\" (localizedPanelLabel(\"Hypershade\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"hyperShadePanel\" -l (localizedPanelLabel(\"Hypershade\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypershade\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"visorPanel\" (localizedPanelLabel(\"Visor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"visorPanel\" -l (localizedPanelLabel(\"Visor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n"
		+ "\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Visor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"nodeEditorPanel\" (localizedPanelLabel(\"Node Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"nodeEditorPanel\" -l (localizedPanelLabel(\"Node Editor\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"NodeEditorEd\");\n            nodeEditor -e \n                -allAttributes 0\n                -allNodes 0\n                -autoSizeNodes 1\n                -consistentNameSize 1\n                -createNodeCommand \"nodeEdCreateNodeCommand\" \n                -defaultPinnedState 0\n                -additiveGraphingMode 0\n                -settingsChangedCallback \"nodeEdSyncControls\" \n                -traversalDepthLimit -1\n                -keyPressCommand \"nodeEdKeyPressCommand\" \n                -nodeTitleMode \"name\" \n                -gridSnap 0\n"
		+ "                -gridVisibility 1\n                -popupMenuScript \"nodeEdBuildPanelMenus\" \n                -showNamespace 1\n                -showShapes 1\n                -showSGShapes 0\n                -showTransforms 1\n                -useAssets 1\n                -syncedSelection 1\n                -extendToShapes 1\n                -activeTab -1\n                -editorMode \"default\" \n                $editorName;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Node Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"NodeEditorEd\");\n            nodeEditor -e \n                -allAttributes 0\n                -allNodes 0\n                -autoSizeNodes 1\n                -consistentNameSize 1\n                -createNodeCommand \"nodeEdCreateNodeCommand\" \n                -defaultPinnedState 0\n                -additiveGraphingMode 0\n                -settingsChangedCallback \"nodeEdSyncControls\" \n                -traversalDepthLimit -1\n                -keyPressCommand \"nodeEdKeyPressCommand\" \n"
		+ "                -nodeTitleMode \"name\" \n                -gridSnap 0\n                -gridVisibility 1\n                -popupMenuScript \"nodeEdBuildPanelMenus\" \n                -showNamespace 1\n                -showShapes 1\n                -showSGShapes 0\n                -showTransforms 1\n                -useAssets 1\n                -syncedSelection 1\n                -extendToShapes 1\n                -activeTab -1\n                -editorMode \"default\" \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"createNodePanel\" (localizedPanelLabel(\"Create Node\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"createNodePanel\" -l (localizedPanelLabel(\"Create Node\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Create Node\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n"
		+ "\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"polyTexturePlacementPanel\" (localizedPanelLabel(\"UV Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"polyTexturePlacementPanel\" -l (localizedPanelLabel(\"UV Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"UV Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\tif ($useSceneConfig) {\n\t\tscriptedPanel -e -to $panelName;\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"renderWindowPanel\" (localizedPanelLabel(\"Render View\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"renderWindowPanel\" -l (localizedPanelLabel(\"Render View\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Render View\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"blendShapePanel\" (localizedPanelLabel(\"Blend Shape\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\tblendShapePanel -unParent -l (localizedPanelLabel(\"Blend Shape\")) -mbv $menusOkayInPanels ;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tblendShapePanel -edit -l (localizedPanelLabel(\"Blend Shape\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynRelEdPanel\" (localizedPanelLabel(\"Dynamic Relationships\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"dynRelEdPanel\" -l (localizedPanelLabel(\"Dynamic Relationships\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dynamic Relationships\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n"
		+ "\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"relationshipPanel\" (localizedPanelLabel(\"Relationship Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"relationshipPanel\" -l (localizedPanelLabel(\"Relationship Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Relationship Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"referenceEditorPanel\" (localizedPanelLabel(\"Reference Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"referenceEditorPanel\" -l (localizedPanelLabel(\"Reference Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Reference Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"componentEditorPanel\" (localizedPanelLabel(\"Component Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"componentEditorPanel\" -l (localizedPanelLabel(\"Component Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Component Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynPaintScriptedPanelType\" (localizedPanelLabel(\"Paint Effects\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"dynPaintScriptedPanelType\" -l (localizedPanelLabel(\"Paint Effects\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Paint Effects\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"scriptEditorPanel\" (localizedPanelLabel(\"Script Editor\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"scriptEditorPanel\" -l (localizedPanelLabel(\"Script Editor\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Script Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"profilerPanel\" (localizedPanelLabel(\"Profiler Tool\")) `;\n\tif (\"\" == $panelName) {\n\t\tif ($useSceneConfig) {\n\t\t\t$panelName = `scriptedPanel -unParent  -type \"profilerPanel\" -l (localizedPanelLabel(\"Profiler Tool\")) -mbv $menusOkayInPanels `;\n\t\t}\n\t} else {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Profiler Tool\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\tif ($useSceneConfig) {\n        string $configName = `getPanel -cwl (localizedPanelLabel(\"Current Layout\"))`;\n        if (\"\" != $configName) {\n\t\t\tpanelConfiguration -edit -label (localizedPanelLabel(\"Current Layout\")) \n\t\t\t\t-defaultImage \"vacantCell.xP:/\"\n\t\t\t\t-image \"\"\n\t\t\t\t-sc false\n\t\t\t\t-configString \"global string $gMainPane; paneLayout -e -cn \\\"vertical2\\\" -ps 1 15 100 -ps 2 85 100 $gMainPane;\"\n\t\t\t\t-removeAllPanels\n\t\t\t\t-ap false\n\t\t\t\t\t(localizedPanelLabel(\"Outliner\")) \n\t\t\t\t\t\"outlinerPanel\"\n"
		+ "\t\t\t\t\t\"$panelName = `outlinerPanel -unParent -l (localizedPanelLabel(\\\"Outliner\\\")) -mbv $menusOkayInPanels `;\\n$editorName = $panelName;\\noutlinerEditor -e \\n    -docTag \\\"isolOutln_fromSeln\\\" \\n    -showShapes 0\\n    -showReferenceNodes 1\\n    -showReferenceMembers 1\\n    -showAttributes 0\\n    -showConnected 0\\n    -showAnimCurvesOnly 0\\n    -showMuteInfo 0\\n    -organizeByLayer 1\\n    -showAnimLayerWeight 1\\n    -autoExpandLayers 1\\n    -autoExpand 0\\n    -showDagOnly 1\\n    -showAssets 1\\n    -showContainedOnly 1\\n    -showPublishedAsConnected 0\\n    -showContainerContents 1\\n    -ignoreDagHierarchy 0\\n    -expandConnections 0\\n    -showUpstreamCurves 1\\n    -showUnitlessCurves 1\\n    -showCompounds 1\\n    -showLeafs 1\\n    -showNumericAttrsOnly 0\\n    -highlightActive 1\\n    -autoSelectNewObjects 0\\n    -doNotSelectNewObjects 0\\n    -dropIsParent 1\\n    -transmitFilters 0\\n    -setFilter \\\"defaultSetFilter\\\" \\n    -showSetMembers 1\\n    -allowMultiSelection 1\\n    -alwaysToggleSelect 0\\n    -directSelect 0\\n    -displayMode \\\"DAG\\\" \\n    -expandObjects 0\\n    -setsIgnoreFilters 1\\n    -containersIgnoreFilters 0\\n    -editAttrName 0\\n    -showAttrValues 0\\n    -highlightSecondary 0\\n    -showUVAttrsOnly 0\\n    -showTextureNodesOnly 0\\n    -attrAlphaOrder \\\"default\\\" \\n    -animLayerFilterOptions \\\"allAffecting\\\" \\n    -sortOrder \\\"none\\\" \\n    -longNames 0\\n    -niceNames 1\\n    -showNamespace 1\\n    -showPinIcons 0\\n    -mapMotionTrails 0\\n    -ignoreHiddenAttribute 0\\n    -ignoreOutlinerColor 0\\n    $editorName\"\n"
		+ "\t\t\t\t\t\"outlinerPanel -edit -l (localizedPanelLabel(\\\"Outliner\\\")) -mbv $menusOkayInPanels  $panelName;\\n$editorName = $panelName;\\noutlinerEditor -e \\n    -docTag \\\"isolOutln_fromSeln\\\" \\n    -showShapes 0\\n    -showReferenceNodes 1\\n    -showReferenceMembers 1\\n    -showAttributes 0\\n    -showConnected 0\\n    -showAnimCurvesOnly 0\\n    -showMuteInfo 0\\n    -organizeByLayer 1\\n    -showAnimLayerWeight 1\\n    -autoExpandLayers 1\\n    -autoExpand 0\\n    -showDagOnly 1\\n    -showAssets 1\\n    -showContainedOnly 1\\n    -showPublishedAsConnected 0\\n    -showContainerContents 1\\n    -ignoreDagHierarchy 0\\n    -expandConnections 0\\n    -showUpstreamCurves 1\\n    -showUnitlessCurves 1\\n    -showCompounds 1\\n    -showLeafs 1\\n    -showNumericAttrsOnly 0\\n    -highlightActive 1\\n    -autoSelectNewObjects 0\\n    -doNotSelectNewObjects 0\\n    -dropIsParent 1\\n    -transmitFilters 0\\n    -setFilter \\\"defaultSetFilter\\\" \\n    -showSetMembers 1\\n    -allowMultiSelection 1\\n    -alwaysToggleSelect 0\\n    -directSelect 0\\n    -displayMode \\\"DAG\\\" \\n    -expandObjects 0\\n    -setsIgnoreFilters 1\\n    -containersIgnoreFilters 0\\n    -editAttrName 0\\n    -showAttrValues 0\\n    -highlightSecondary 0\\n    -showUVAttrsOnly 0\\n    -showTextureNodesOnly 0\\n    -attrAlphaOrder \\\"default\\\" \\n    -animLayerFilterOptions \\\"allAffecting\\\" \\n    -sortOrder \\\"none\\\" \\n    -longNames 0\\n    -niceNames 1\\n    -showNamespace 1\\n    -showPinIcons 0\\n    -mapMotionTrails 0\\n    -ignoreHiddenAttribute 0\\n    -ignoreOutlinerColor 0\\n    $editorName\"\n"
		+ "\t\t\t\t-ap false\n\t\t\t\t\t(localizedPanelLabel(\"Persp View\")) \n\t\t\t\t\t\"modelPanel\"\n"
		+ "\t\t\t\t\t\"$panelName = `modelPanel -unParent -l (localizedPanelLabel(\\\"Persp View\\\")) -mbv $menusOkayInPanels `;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera persp` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"smoothShaded\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 0\\n    -headsUpDisplay 1\\n    -holdOuts 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 0\\n    -backfaceCulling 0\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 0\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 16384\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -depthOfFieldPreview 1\\n    -maxConstantTransparency 1\\n    -rendererName \\\"vp2Renderer\\\" \\n    -objectFilterShowInHUD 1\\n    -isFiltered 0\\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"frontAndBackCull\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 1\\n    -cameras 1\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 1\\n    -imagePlane 1\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -particleInstancers 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -pluginShapes 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -clipGhosts 1\\n    -greasePencils 1\\n    -shadows 0\\n    -captureSequenceNumber -1\\n    -width 1379\\n    -height 785\\n    -sceneRenderFilter 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName;\\nmodelEditor -e \\n    -pluginObjects \\\"gpuCacheDisplayFilter\\\" 1 \\n    $editorName\"\n"
		+ "\t\t\t\t\t\"modelPanel -edit -l (localizedPanelLabel(\\\"Persp View\\\")) -mbv $menusOkayInPanels  $panelName;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera persp` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"smoothShaded\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 0\\n    -headsUpDisplay 1\\n    -holdOuts 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 0\\n    -backfaceCulling 0\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 0\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 16384\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -depthOfFieldPreview 1\\n    -maxConstantTransparency 1\\n    -rendererName \\\"vp2Renderer\\\" \\n    -objectFilterShowInHUD 1\\n    -isFiltered 0\\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"frontAndBackCull\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 1\\n    -cameras 1\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 1\\n    -imagePlane 1\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -particleInstancers 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -pluginShapes 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -clipGhosts 1\\n    -greasePencils 1\\n    -shadows 0\\n    -captureSequenceNumber -1\\n    -width 1379\\n    -height 785\\n    -sceneRenderFilter 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName;\\nmodelEditor -e \\n    -pluginObjects \\\"gpuCacheDisplayFilter\\\" 1 \\n    $editorName\"\n"
		+ "\t\t\t\t$configName;\n\n            setNamedPanelLayout (localizedPanelLabel(\"Current Layout\"));\n        }\n\n        panelHistory -e -clear mainPanelHistory;\n        setFocus `paneLayout -q -p1 $gMainPane`;\n        sceneUIReplacement -deleteRemaining;\n        sceneUIReplacement -clear;\n\t}\n\n\ngrid -spacing 5 -size 12 -divisions 5 -displayAxes yes -displayGridLines yes -displayDivisionLines yes -displayPerspectiveLabels no -displayOrthographicLabels no -displayAxesBold yes -perspectiveLabelPosition axis -orthographicLabelPosition edge;\nviewManip -drawCompass 0 -compassAngle 0 -frontParameters \"\" -homeParameters \"\" -selectionLockParameters \"\";\n}\n");
	setAttr ".st" 3;
createNode script -n "sceneConfigurationScriptNode";
	rename -uid "9F7F11E3-446D-A538-3748-A0A87430CD2E";
	setAttr ".b" -type "string" "playbackOptions -min 1 -max 120 -ast 1 -aet 200 ";
	setAttr ".st" 6;
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 22 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surface" "Particles" "Particle Instance" "Fluids" "Strokes" "Image Planes" "UI" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Hair Systems" "Follicles" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 22 0 1 1 1 1 1
		 1 1 1 0 0 0 0 0 0 0 0 0
		 0 0 0 0 ;
	setAttr ".fprt" yes;
select -ne :renderPartition;
	setAttr -s 4 ".st";
select -ne :renderGlobalsList1;
select -ne :defaultShaderList1;
	setAttr -s 6 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderingList1;
select -ne :initialShadingGroup;
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
connectAttr "polyTweakUV3.out" "pCylinderShape1.i";
connectAttr "polyTweakUV3.uvtk[0]" "pCylinderShape1.uvst[0].uvtw";
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "BushSG.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "BerrySG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "BushSG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "BerrySG.message" ":defaultLightSet.message";
connectAttr "layerManager.dli[0]" "defaultLayer.id";
connectAttr "renderLayerManager.rlmi[0]" "defaultRenderLayer.rlid";
connectAttr "lambert5.oc" "BushSG.ss";
connectAttr "BushSG.msg" "materialInfo1.sg";
connectAttr "lambert5.msg" "materialInfo1.m";
connectAttr "blinn1.oc" "BerrySG.ss";
connectAttr "BerrySG.msg" "materialInfo2.sg";
connectAttr "blinn1.msg" "materialInfo2.m";
connectAttr "polyCylinder1.out" "polySplit1.ip";
connectAttr "polySplit1.out" "polySplit2.ip";
connectAttr "polySplit2.out" "polySplit3.ip";
connectAttr "polySplit3.out" "polySplit4.ip";
connectAttr "polySplit4.out" "polyTweak1.ip";
connectAttr "polyTweak1.out" "polySplit5.ip";
connectAttr "polySplit5.out" "polyTweak2.ip";
connectAttr "polyTweak2.out" "polySplit6.ip";
connectAttr "polySplit6.out" "polyTweak3.ip";
connectAttr "polyTweak3.out" "polySplit7.ip";
connectAttr "polyTweak4.out" "polySoftEdge1.ip";
connectAttr "pCylinderShape1.wm" "polySoftEdge1.mp";
connectAttr "polySplit7.out" "polyTweak4.ip";
connectAttr "polyTweak5.out" "polyMergeUV1.ip";
connectAttr "polySoftEdge1.out" "polyTweak5.ip";
connectAttr "polyMergeUV1.out" "polyMapCut1.ip";
connectAttr "polyMapCut1.out" "polyMapCut2.ip";
connectAttr "polyMapCut2.out" "Unfold3DUnfold1.im";
connectAttr "Unfold3DUnfold1.om" "polyTweakUV1.ip";
connectAttr "polyTweakUV1.out" "polyMergeUV2.ip";
connectAttr "polyMergeUV2.out" "Unfold3DUnfold2.im";
connectAttr "Unfold3DUnfold2.om" "polyMapCut3.ip";
connectAttr "polyMapCut3.out" "polyTweakUV2.ip";
connectAttr "polyTweakUV2.out" "polyMergeUV3.ip";
connectAttr "polyMergeUV3.out" "Unfold3DUnfold3.im";
connectAttr "Unfold3DUnfold3.om" "polyMapCut4.ip";
connectAttr "polyMapCut4.out" "Unfold3DUnfold4.im";
connectAttr "Unfold3DUnfold4.om" "polyTweakUV3.ip";
connectAttr "BushSG.pa" ":renderPartition.st" -na;
connectAttr "BerrySG.pa" ":renderPartition.st" -na;
connectAttr "lambert5.msg" ":defaultShaderList1.s" -na;
connectAttr "blinn1.msg" ":defaultShaderList1.s" -na;
connectAttr "defaultRenderLayer.msg" ":defaultRenderingList1.r" -na;
connectAttr "pCylinderShape1.iog" ":initialShadingGroup.dsm" -na;
// End of Asset_Blé.ma
