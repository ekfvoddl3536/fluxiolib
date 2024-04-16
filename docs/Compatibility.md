# A, TEST PASSED
List of systems and .NET runtimes that passed testing.  
OS                                    | OS Version              | Architecture            | .NET
--------------------------------------|-------------------------|-------------------------|--------
Windows 11                            | 22631.3296              | x64                     | 8.0 
Windows 11                            | 22631.3296              | x64                     | 8.0 AOT 
Ubuntu                                | 22.04.4 LTS             | x64                     | 8.0 AOT 



# B, EXPECTED TO BE COMPATIBLE
List of systems not tested, but expected to be compatible.  
The **.NET runtime** requires at least version `.NET 8`.  
  
## Windows
OS                                    | OS Version              | Architecture            
--------------------------------------|-------------------------|-------------------------
Windows 10 Client                     | 1607+                   | x64, Arm64              
Windows 11                            | 22000+                  | x64, Arm64              
Windows Server                        | 2012+                   | x64                     
Windows Server Core                   | 2012+                   | x64                     

## Linux
OS                                    | OS Version              | Architecture            
--------------------------------------|-------------------------|-------------------------
Alpine Linux                          | 3.16+                   | x64, Arm64              
Debian                                | 11+                     | x64, Arm64              
Fedora                                | 38+                     | x64                     
openSUSE                              | 15+                     | x64                     
Oracle Linux                          | 8+                      | x64                     
Red Hat Enterprise Linux              | 8+                      | x64, Arm64              
SUSE Enterprise Linux (SLES)          | 12 SP5+                 | x64                     
Ubuntu                                | 20.04+                  | x64, Arm64              

## macOS
OS                                    | OS Version              | Architecture            
--------------------------------------|-------------------------|-------------------------
macOS                                 | 12.0+                   | x64, Arm64              



# C, UNCERTAIN ABOUT COMPATIBILITY
List of systems expected to be compatible, but very uncertain.  
The **.NET runtime** requires at least version `.NET 8`.  
  
## Windows
OS                                    | OS Version              | Architecture            
--------------------------------------|-------------------------|-------------------------
Nano Server                           | 1809+                   | x64                     

## Android
OS                                    | OS Version              | Architecture            
--------------------------------------|-------------------------|-------------------------
Android                               | API 21+                 | x64, Arm64              

## iOS / tvOS / MacCatalyst
OS                                    | OS Version              | Architecture            
--------------------------------------|-------------------------|-------------------------
iOS                                   | 12.0+                   | Arm64                   
iOS Simulator                         | 12.0+                   | x64, Arm64              
tvOS                                  | 12.0+                   | Arm64                   
tvOS Simulator                        | 12.0+                   | x64, Arm64              
MacCatalyst                           | 12.0+                   | x64, Arm64              



# F, NOT COMPATIBILITY
All 32-bit architectures are not supported.  
32-bit Processes or 32-bit operating systems are also not supported.  
  
OS                                    | OS Version              | Architecture            
--------------------------------------|-------------------------|-------------------------
Any*                                  | Any*                    | x86, Arm32
