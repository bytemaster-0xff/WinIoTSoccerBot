G21 ; All units in mm
M80 ; Turn on Optional Peripherals Board at LMN

; Raster data will always precede vector data           
; Default Cut Feedrate 300 mm per minute
; Default Move Feedrate 2000 mm per minute
; Default Laser Intensity 100 percent
G28 XY; home X and Y



M5 ;turn the laser off

;(************************************************************)
;(***** Layer: 100 [feed=300]                            *****)
;(***** Laser Power: 100                                 *****)
;(***** Feed Rate: 300.0                                 *****)
;(************************************************************)
;(MSG,Starting layer '100 [feed=300]')

G00 X7.973 Y112.981 F2000 
G01 X7.973 Y8.981 S100.00 F300 B0 D0
G01 X97.973 Y8.981
G01 X97.973 Y112.981
G01 X7.973 Y112.981
G01 X7.973 Y112.981
G00 X20.923 Y104.894 F2000 
G02 X21.866 Y104.707 I0.000 J-2.463S100.00 F300 B0 D0
G02 X22.665 Y104.173 I-0.943 J-2.276
G02 X23.199 Y103.374 I-1.742 J-1.742
G02 X23.386 Y102.431 I-2.276 J-0.943
G02 X23.199 Y101.488 I-2.463 J-0.000
G02 X22.665 Y100.689 I-2.276 J0.943
G02 X21.866 Y100.155 I-1.742 J1.742
G02 X20.923 Y99.968 I-0.943 J2.276
G02 X19.980 Y100.155 I-0.000 J2.463
G02 X19.181 Y100.689 I0.943 J2.276
G02 X18.647 Y101.488 I1.742 J1.742
G02 X18.460 Y102.431 I2.276 J0.943
G02 X18.647 Y103.374 I2.463 J0.000
G02 X19.181 Y104.173 I2.276 J-0.943
G02 X19.980 Y104.707 I1.742 J-1.742
G02 X20.923 Y104.894 I0.943 J-2.276
G01 X20.923 Y104.894
G00 X84.923 Y104.894 F2000 
G02 X85.866 Y104.707 I0.000 J-2.463S100.00 F300 B0 D0
G02 X86.665 Y104.173 I-0.943 J-2.276
G02 X87.199 Y103.374 I-1.742 J-1.742
G02 X87.386 Y102.431 I-2.276 J-0.943
G02 X87.199 Y101.488 I-2.463 J-0.000
G02 X86.665 Y100.689 I-2.276 J0.943
G02 X85.866 Y100.155 I-1.742 J1.742
G02 X84.923 Y99.968 I-0.943 J2.276
G02 X83.980 Y100.155 I-0.000 J2.463
G02 X83.181 Y100.689 I0.943 J2.276
G02 X82.647 Y101.488 I1.742 J1.742
G02 X82.460 Y102.431 I2.276 J0.943
G02 X82.647 Y103.374 I2.463 J0.000
G02 X83.181 Y104.173 I2.276 J-0.943
G02 X83.980 Y104.707 I1.742 J-1.742
G02 X84.923 Y104.894 I0.943 J-2.276
G01 X84.923 Y104.894
G00 X28.531 Y81.179 F2000 
G02 X29.034 Y81.079 I0.000 J-1.314S100.00 F300 B0 D0
G02 X29.460 Y80.795 I-0.502 J-1.214
G02 X29.745 Y80.369 I-0.929 J-0.929
G02 X29.845 Y79.866 I-1.214 J-0.503
G02 X29.745 Y79.363 I-1.314 J0.000
G02 X29.460 Y78.937 I-1.214 J0.503
G02 X29.034 Y78.652 I-0.929 J0.929
G02 X28.531 Y78.552 I-0.502 J1.214
G02 X27.602 Y78.937 I0.000 J1.314
G02 X27.217 Y79.866 I0.929 J0.929
G02 X27.602 Y80.795 I1.314 J-0.000
G02 X28.531 Y81.179 I0.929 J-0.929
G01 X28.531 Y81.179
G00 X77.531 Y81.179 F2000 
G02 X78.460 Y80.795 I-0.000 J-1.314S100.00 F300 B0 D0
G02 X78.845 Y79.866 I-0.929 J-0.929
G02 X78.460 Y78.937 I-1.314 J0.000
G02 X77.531 Y78.552 I-0.929 J0.929
G02 X76.602 Y78.937 I0.000 J1.314
G02 X76.218 Y79.866 I0.929 J0.929
G02 X76.602 Y80.795 I1.314 J-0.000
G02 X77.531 Y81.179 I0.929 J-0.929
G01 X77.531 Y81.179
G00 X28.531 Y23.179 F2000 
G02 X29.034 Y23.080 I0.000 J-1.314S100.00 F300 B0 D0
G02 X29.460 Y22.795 I-0.502 J-1.214
G02 X29.745 Y22.369 I-0.929 J-0.929
G02 X29.845 Y21.866 I-1.214 J-0.503
G02 X29.745 Y21.363 I-1.314 J0.000
G02 X29.460 Y20.937 I-1.214 J0.503
G02 X29.034 Y20.652 I-0.929 J0.929
G02 X28.531 Y20.552 I-0.502 J1.214
G02 X27.602 Y20.937 I0.000 J1.314
G02 X27.217 Y21.866 I0.929 J0.929
G02 X27.602 Y22.795 I1.314 J-0.000
G02 X28.531 Y23.179 I0.929 J-0.929
G01 X28.531 Y23.179
G00 X77.531 Y23.179 F2000 
G02 X78.460 Y22.795 I-0.000 J-1.314S100.00 F300 B0 D0
G02 X78.845 Y21.866 I-0.929 J-0.929
G02 X78.460 Y20.937 I-1.314 J0.000
G02 X77.531 Y20.552 I-0.929 J0.929
G02 X76.602 Y20.937 I0.000 J1.314
G02 X76.218 Y21.866 I0.929 J0.929
G02 X76.602 Y22.795 I1.314 J-0.000
G02 X77.531 Y23.179 I0.929 J-0.929
G01 X77.531 Y23.179
G00 X20.923 Y16.894 F2000 
G02 X21.866 Y16.707 I0.000 J-2.463S100.00 F300 B0 D0
G02 X22.665 Y16.173 I-0.943 J-2.276
G02 X23.199 Y15.374 I-1.742 J-1.742
G02 X23.386 Y14.431 I-2.276 J-0.943
G02 X23.199 Y13.488 I-2.463 J-0.000
G02 X22.665 Y12.689 I-2.276 J0.943
G02 X21.866 Y12.155 I-1.742 J1.742
G02 X20.923 Y11.968 I-0.943 J2.276
G02 X19.980 Y12.155 I-0.000 J2.463
G02 X19.181 Y12.689 I0.943 J2.276
G02 X18.647 Y13.488 I1.742 J1.742
G02 X18.460 Y14.431 I2.276 J0.943
G02 X18.647 Y15.374 I2.463 J0.000
G02 X19.181 Y16.173 I2.276 J-0.943
G02 X19.980 Y16.707 I1.742 J-1.742
G02 X20.923 Y16.894 I0.943 J-2.276
G01 X20.923 Y16.894
G00 X84.923 Y16.894 F2000 
G02 X85.866 Y16.707 I0.000 J-2.463S100.00 F300 B0 D0
G02 X86.665 Y16.173 I-0.943 J-2.276
G02 X87.199 Y15.374 I-1.742 J-1.742
G02 X87.386 Y14.431 I-2.276 J-0.943
G02 X87.199 Y13.488 I-2.463 J-0.000
G02 X86.665 Y12.689 I-2.276 J0.943
G02 X85.866 Y12.155 I-1.742 J1.742
G02 X84.923 Y11.968 I-0.943 J2.276
G02 X83.980 Y12.155 I-0.000 J2.463
G02 X83.181 Y12.689 I0.943 J2.276
G02 X82.647 Y13.488 I1.742 J1.742
G02 X82.460 Y14.431 I2.276 J0.943
G02 X82.647 Y15.374 I2.463 J0.000
G02 X83.181 Y16.173 I2.276 J-0.943
G02 X83.980 Y16.707 I1.742 J-1.742
G02 X84.923 Y16.894 I0.943 J-2.276
G01 X84.923 Y16.894
M5 ;turn the laser off
M5 ;turn the laser off

;(************************************************************)
;(***** Layer: 100 [feed=300]                            *****)
;(***** Laser Power: 100                                 *****)
;(***** Feed Rate: 300.0                                 *****)
;(************************************************************)
;(MSG,Starting layer '100 [feed=300]')

G00 X102.461 Y112.980 F2000 
G01 X102.461 Y8.980 S100.00 F300 B0 D0
G01 X192.461 Y8.980
G01 X192.461 Y112.980
G01 X102.461 Y112.980
G01 X102.461 Y112.980
G00 X115.411 Y104.893 F2000 
G02 X116.354 Y104.706 I0.000 J-2.463S100.00 F300 B0 D0
G02 X117.153 Y104.172 I-0.943 J-2.276
G02 X117.687 Y103.373 I-1.742 J-1.742
G02 X117.874 Y102.430 I-2.276 J-0.943
G02 X117.687 Y101.487 I-2.463 J-0.000
G02 X117.153 Y100.688 I-2.276 J0.943
G02 X116.354 Y100.154 I-1.742 J1.742
G02 X115.411 Y99.967 I-0.943 J2.276
G02 X114.468 Y100.154 I-0.000 J2.463
G02 X113.669 Y100.688 I0.943 J2.276
G02 X113.135 Y101.487 I1.742 J1.742
G02 X112.948 Y102.430 I2.276 J0.943
G02 X113.135 Y103.373 I2.463 J0.000
G02 X113.669 Y104.172 I2.276 J-0.943
G02 X114.468 Y104.706 I1.742 J-1.742
G02 X115.411 Y104.893 I0.943 J-2.276
G01 X115.411 Y104.893
G00 X179.411 Y104.893 F2000 
G02 X180.354 Y104.706 I0.000 J-2.463S100.00 F300 B0 D0
G02 X181.153 Y104.172 I-0.943 J-2.276
G02 X181.687 Y103.373 I-1.742 J-1.742
G02 X181.874 Y102.430 I-2.276 J-0.943
G02 X181.687 Y101.487 I-2.463 J-0.000
G02 X181.153 Y100.688 I-2.276 J0.943
G02 X180.354 Y100.154 I-1.742 J1.742
G02 X179.411 Y99.967 I-0.943 J2.276
G02 X178.468 Y100.154 I-0.000 J2.463
G02 X177.669 Y100.688 I0.943 J2.276
G02 X177.135 Y101.487 I1.742 J1.742
G02 X176.948 Y102.430 I2.276 J0.943
G02 X177.135 Y103.373 I2.463 J0.000
G02 X177.669 Y104.172 I2.276 J-0.943
G02 X178.468 Y104.706 I1.742 J-1.742
G02 X179.411 Y104.893 I0.943 J-2.276
G01 X179.411 Y104.893
G00 X123.019 Y81.178 F2000 
G02 X123.522 Y81.078 I0.000 J-1.314S100.00 F300 B0 D0
G02 X123.948 Y80.794 I-0.502 J-1.214
G02 X124.233 Y80.368 I-0.929 J-0.929
G02 X124.333 Y79.865 I-1.214 J-0.503
G02 X124.233 Y79.362 I-1.314 J0.000
G02 X123.948 Y78.936 I-1.214 J0.503
G02 X123.522 Y78.651 I-0.929 J0.929
G02 X123.019 Y78.551 I-0.502 J1.214
G02 X122.090 Y78.936 I0.000 J1.314
G02 X121.705 Y79.865 I0.929 J0.929
G02 X122.090 Y80.794 I1.314 J-0.000
G02 X123.019 Y81.178 I0.929 J-0.929
G01 X123.019 Y81.178
G00 X172.019 Y81.178 F2000 
G02 X172.948 Y80.794 I-0.000 J-1.314S100.00 F300 B0 D0
G02 X173.333 Y79.865 I-0.929 J-0.929
G02 X172.948 Y78.936 I-1.314 J0.000
G02 X172.019 Y78.551 I-0.929 J0.929
G02 X171.090 Y78.936 I0.000 J1.314
G02 X170.706 Y79.865 I0.929 J0.929
G02 X171.090 Y80.794 I1.314 J-0.000
G02 X172.019 Y81.178 I0.929 J-0.929
G01 X172.019 Y81.178
G00 X123.019 Y23.178 F2000 
G02 X123.522 Y23.078 I0.000 J-1.314S100.00 F300 B0 D0
G02 X123.948 Y22.794 I-0.502 J-1.214
G02 X124.233 Y22.368 I-0.929 J-0.929
G02 X124.333 Y21.865 I-1.214 J-0.503
G02 X124.233 Y21.362 I-1.314 J0.000
G02 X123.948 Y20.936 I-1.214 J0.503
G02 X123.522 Y20.651 I-0.929 J0.929
G02 X123.019 Y20.551 I-0.502 J1.214
G02 X122.090 Y20.936 I0.000 J1.314
G02 X121.705 Y21.865 I0.929 J0.929
G02 X122.090 Y22.794 I1.314 J-0.000
G02 X123.019 Y23.178 I0.929 J-0.929
G01 X123.019 Y23.178
G00 X172.019 Y23.178 F2000 
G02 X172.948 Y22.794 I-0.000 J-1.314S100.00 F300 B0 D0
G02 X173.333 Y21.865 I-0.929 J-0.929
G02 X172.948 Y20.936 I-1.314 J0.000
G02 X172.019 Y20.551 I-0.929 J0.929
G02 X171.090 Y20.936 I0.000 J1.314
G02 X170.706 Y21.865 I0.929 J0.929
G02 X171.090 Y22.794 I1.314 J-0.000
G02 X172.019 Y23.178 I0.929 J-0.929
G01 X172.019 Y23.178
G00 X115.411 Y16.893 F2000 
G02 X116.354 Y16.706 I0.000 J-2.463S100.00 F300 B0 D0
G02 X117.153 Y16.172 I-0.943 J-2.276
G02 X117.687 Y15.373 I-1.742 J-1.742
G02 X117.874 Y14.430 I-2.276 J-0.943
G02 X117.687 Y13.487 I-2.463 J-0.000
G02 X117.153 Y12.688 I-2.276 J0.943
G02 X116.354 Y12.154 I-1.742 J1.742
G02 X115.411 Y11.967 I-0.943 J2.276
G02 X114.468 Y12.154 I-0.000 J2.463
G02 X113.669 Y12.688 I0.943 J2.276
G02 X113.135 Y13.487 I1.742 J1.742
G02 X112.948 Y14.430 I2.276 J0.943
G02 X113.135 Y15.373 I2.463 J0.000
G02 X113.669 Y16.172 I2.276 J-0.943
G02 X114.468 Y16.706 I1.742 J-1.742
G02 X115.411 Y16.893 I0.943 J-2.276
G01 X115.411 Y16.893
G00 X179.411 Y16.893 F2000 
G02 X180.354 Y16.706 I0.000 J-2.463S100.00 F300 B0 D0
G02 X181.153 Y16.172 I-0.943 J-2.276
G02 X181.687 Y15.373 I-1.742 J-1.742
G02 X181.874 Y14.430 I-2.276 J-0.943
G02 X181.687 Y13.487 I-2.463 J-0.000
G02 X181.153 Y12.688 I-2.276 J0.943
G02 X180.354 Y12.154 I-1.742 J1.742
G02 X179.411 Y11.967 I-0.943 J2.276
G02 X178.468 Y12.154 I-0.000 J2.463
G02 X177.669 Y12.688 I0.943 J2.276
G02 X177.135 Y13.487 I1.742 J1.742
G02 X176.948 Y14.430 I2.276 J0.943
G02 X177.135 Y15.373 I2.463 J0.000
G02 X177.669 Y16.172 I2.276 J-0.943
G02 X178.468 Y16.706 I1.742 J-1.742
G02 X179.411 Y16.893 I0.943 J-2.276
G01 X179.411 Y16.893
M5 ;turn the laser off
M5 ;turn the laser off

;(************************************************************)
;(***** Layer: 100 [feed=300]                            *****)
;(***** Laser Power: 100                                 *****)
;(***** Feed Rate: 300.0                                 *****)
;(************************************************************)
;(MSG,Starting layer '100 [feed=300]')

G00 X197.913 Y113.242 F2000 
G01 X197.913 Y9.242 S100.00 F300 B0 D0
G01 X287.913 Y9.242
G01 X287.913 Y113.242
G01 X197.913 Y113.242
G01 X197.913 Y113.242
G00 X210.863 Y105.155 F2000 
G02 X211.805 Y104.968 I0.000 J-2.463S100.00 F300 B0 D0
G02 X212.604 Y104.434 I-0.943 J-2.276
G02 X213.138 Y103.635 I-1.742 J-1.742
G02 X213.326 Y102.692 I-2.276 J-0.943
G02 X213.138 Y101.749 I-2.463 J-0.000
G02 X212.604 Y100.950 I-2.276 J0.943
G02 X211.805 Y100.416 I-1.742 J1.742
G02 X210.863 Y100.229 I-0.943 J2.276
G02 X209.920 Y100.416 I-0.000 J2.463
G02 X209.121 Y100.950 I0.943 J2.276
G02 X208.587 Y101.749 I1.742 J1.742
G02 X208.399 Y102.692 I2.276 J0.943
G02 X208.587 Y103.635 I2.463 J0.000
G02 X209.121 Y104.434 I2.276 J-0.943
G02 X209.920 Y104.968 I1.742 J-1.742
G02 X210.863 Y105.155 I0.943 J-2.276
G01 X210.863 Y105.155
G00 X274.863 Y105.155 F2000 
G02 X275.805 Y104.968 I0.000 J-2.463S100.00 F300 B0 D0
G02 X276.604 Y104.434 I-0.943 J-2.276
G02 X277.138 Y103.635 I-1.742 J-1.742
G02 X277.326 Y102.692 I-2.276 J-0.943
G02 X277.138 Y101.749 I-2.463 J-0.000
G02 X276.604 Y100.950 I-2.276 J0.943
G02 X275.805 Y100.416 I-1.742 J1.742
G02 X274.863 Y100.229 I-0.943 J2.276
G02 X273.920 Y100.416 I-0.000 J2.463
G02 X273.121 Y100.950 I0.943 J2.276
G02 X272.587 Y101.749 I1.742 J1.742
G02 X272.399 Y102.692 I2.276 J0.943
G02 X272.587 Y103.635 I2.463 J0.000
G02 X273.121 Y104.434 I2.276 J-0.943
G02 X273.920 Y104.968 I1.742 J-1.742
G02 X274.863 Y105.155 I0.943 J-2.276
G01 X274.863 Y105.155
G00 X218.470 Y81.440 F2000 
G02 X218.973 Y81.341 I0.000 J-1.314S100.00 F300 B0 D0
G02 X219.400 Y81.056 I-0.502 J-1.214
G02 X219.684 Y80.630 I-0.929 J-0.929
G02 X219.785 Y80.127 I-1.214 J-0.503
G02 X219.684 Y79.624 I-1.314 J0.000
G02 X219.400 Y79.198 I-1.214 J0.503
G02 X218.973 Y78.913 I-0.929 J0.929
G02 X218.470 Y78.813 I-0.502 J1.214
G02 X217.542 Y79.198 I0.000 J1.314
G02 X217.157 Y80.127 I0.929 J0.929
G02 X217.542 Y81.056 I1.314 J-0.000
G02 X218.470 Y81.440 I0.929 J-0.929
G01 X218.470 Y81.440
G00 X267.471 Y81.440 F2000 
G02 X268.399 Y81.056 I-0.000 J-1.314S100.00 F300 B0 D0
G02 X268.784 Y80.127 I-0.929 J-0.929
G02 X268.399 Y79.198 I-1.314 J0.000
G02 X267.471 Y78.813 I-0.929 J0.929
G02 X266.542 Y79.198 I0.000 J1.314
G02 X266.157 Y80.127 I0.929 J0.929
G02 X266.542 Y81.056 I1.314 J-0.000
G02 X267.471 Y81.440 I0.929 J-0.929
G01 X267.471 Y81.440
G00 X218.470 Y23.440 F2000 
G02 X218.973 Y23.341 I0.000 J-1.314S100.00 F300 B0 D0
G02 X219.400 Y23.056 I-0.502 J-1.214
G02 X219.684 Y22.630 I-0.929 J-0.929
G02 X219.785 Y22.127 I-1.214 J-0.503
G02 X219.684 Y21.624 I-1.314 J0.000
G02 X219.400 Y21.198 I-1.214 J0.503
G02 X218.973 Y20.913 I-0.929 J0.929
G02 X218.470 Y20.813 I-0.502 J1.214
G02 X217.542 Y21.198 I0.000 J1.314
G02 X217.157 Y22.127 I0.929 J0.929
G02 X217.542 Y23.056 I1.314 J-0.000
G02 X218.470 Y23.440 I0.929 J-0.929
G01 X218.470 Y23.440
G00 X267.471 Y23.440 F2000 
G02 X268.399 Y23.056 I-0.000 J-1.314S100.00 F300 B0 D0
G02 X268.784 Y22.127 I-0.929 J-0.929
G02 X268.399 Y21.198 I-1.314 J0.000
G02 X267.471 Y20.813 I-0.929 J0.929
G02 X266.542 Y21.198 I0.000 J1.314
G02 X266.157 Y22.127 I0.929 J0.929
G02 X266.542 Y23.056 I1.314 J-0.000
G02 X267.471 Y23.440 I0.929 J-0.929
G01 X267.471 Y23.440
G00 X210.863 Y17.155 F2000 
G02 X211.805 Y16.968 I0.000 J-2.463S100.00 F300 B0 D0
G02 X212.604 Y16.434 I-0.943 J-2.276
G02 X213.138 Y15.635 I-1.742 J-1.742
G02 X213.326 Y14.692 I-2.276 J-0.943
G02 X213.138 Y13.749 I-2.463 J-0.000
G02 X212.604 Y12.950 I-2.276 J0.943
G02 X211.805 Y12.416 I-1.742 J1.742
G02 X210.863 Y12.229 I-0.943 J2.276
G02 X209.920 Y12.416 I-0.000 J2.463
G02 X209.121 Y12.950 I0.943 J2.276
G02 X208.587 Y13.749 I1.742 J1.742
G02 X208.399 Y14.692 I2.276 J0.943
G02 X208.587 Y15.635 I2.463 J0.000
G02 X209.121 Y16.434 I2.276 J-0.943
G02 X209.920 Y16.968 I1.742 J-1.742
G02 X210.863 Y17.155 I0.943 J-2.276
G01 X210.863 Y17.155
G00 X274.863 Y17.155 F2000 
G02 X275.805 Y16.968 I0.000 J-2.463S100.00 F300 B0 D0
G02 X276.604 Y16.434 I-0.943 J-2.276
G02 X277.138 Y15.635 I-1.742 J-1.742
G02 X277.326 Y14.692 I-2.276 J-0.943
G02 X277.138 Y13.749 I-2.463 J-0.000
G02 X276.604 Y12.950 I-2.276 J0.943
G02 X275.805 Y12.416 I-1.742 J1.742
G02 X274.863 Y12.229 I-0.943 J2.276
G02 X273.920 Y12.416 I-0.000 J2.463
G02 X273.121 Y12.950 I0.943 J2.276
G02 X272.587 Y13.749 I1.742 J1.742
G02 X272.399 Y14.692 I2.276 J0.943
G02 X272.587 Y15.635 I2.463 J0.000
G02 X273.121 Y16.434 I2.276 J-0.943
G02 X273.920 Y16.968 I1.742 J-1.742
G02 X274.863 Y17.155 I0.943 J-2.276
G01 X274.863 Y17.155
M5 ;turn the laser off