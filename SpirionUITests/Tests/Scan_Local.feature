Feature: ScanTest_Local
	Tests containing all scanning on Local

Background:
	Given User is logged in
	Then Create directory on Local machine

Scenario Outline: Given User is trying to scan Single SSN File
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', 'ScanType','<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details '<RightDecision>'
	Then Verify the file is shredded '<FolderPath>'

	Examples:
		| PlayBookName   | PlayBookDescription                   | LeftDecision | RightDecision | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                     | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath               | TestFile              |
		| SSN Shred_AUID | Do not delete or update this PlayBook | User Action  | Shred         | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local SSN Shred_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\Shred | TestData\TestData.csv |

Scenario Outline: Given User is trying to scan Mut SSN File
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Given User is preparing testData at '<ScanTargetType>' for 'CreditCardNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'false'
	Given User is preparing testData at '<ScanTargetType>' for 'TelephoneNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'false'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details for Multiple Classification '<RightDecision>'

	Examples:
		| PlayBookName          | PlayBookDescription                   | LeftDecision   | RightDecision  | LogicName        | LogicType  | LogicTypeOption | ScanItem                                                   | ScanName                            | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                                  | TestFile              |
		| Mutiple Classify_AUID | Do not delete or update this PlayBook | Take No Action | Classification | Automation Logic | Data Types | Contains        | Social Security Number;Telephone Number;Credit Card Number | SDM SD Local Mutiple Classify_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\ClassifyMultipleDataType | TestData\TestData.csv |

Scenario Outline: Given User is trying to scan SSN for Redact resolution
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details '<RightDecision>'
	Then Verify file is redacted sucessfully '<FolderPath>'

	Examples:
		| PlayBookName | PlayBookDescription                   | LeftDecision   | RightDecision | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                  | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                | TestFile              |
		| Redact_AUID  | Do not delete or update this PlayBook | Classification | Redact        | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local Redact_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\Redact | TestData\TestData.csv |

Scenario Outline: Given User is trying to scan SSN for quartine resolution
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details '<RightDecision>'
	Then Verify file is quarantined sucessfully '<QuarantinedFileContent>' '<FolderPath>' '<ScanTargetType>'

	Examples:
		| PlayBookName    | PlayBookDescription                   | LeftDecision   | RightDecision | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                      | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                    | TestFile              | QuarantinedFileContent                                                                                                             |
		| Quarantine_AUID | Do not delete or update this PlayBook | Classification | Quarantine    | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local Quarantine_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\Quarantine | TestData\TestData.csv | The original file, {0}, contained unsecured, personally identifiable information.  It has been auto-quarantined (via workflow) to: |

Scenario Outline: Given User is trying to scan SSN for Execute Script resolution
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User uploads script to the repositiory
		| ScriptName               | ScriptDescription                | Path                       |
		| SR_DeleteFile_Automation | This script is an automation run | TestData\\DeleteScript.bat |
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details 'Script Executed'
	Then Verify the file is shredded '<FolderPath>'

	Examples:
		| PlayBookName        | PlayBookDescription                   | LeftDecision   | RightDecision  | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                         | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                       | TestFile              |
		| ExecuteScripts_AUID | Do not delete or update this PlayBook | Classification | Execute Script | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local ExecuteScript_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\ExecuteScript | TestData\TestData.csv |

Scenario Outline: Given User is trying to scan SSN for Notify resolution
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User creates a custom notification template
		| TemplateName            | Subject                                               | Body                                               |
		| CustomNotification_AUID | CustomNotificationSubject_AUID for verification later | CustomNotificationBody_AUID for verification later |
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details 'No Action Taken'
	Then User selects to click on Notifications
	Then Verify User gets the notification once the scan is completed
	Then Delete the notification from system

	Examples:
		| PlayBookName | PlayBookDescription                   | LeftDecision   | RightDecision | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                  | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                       | TestFile              |
		| Notify_AUID  | Do not delete or update this PlayBook | Classification | Notify        | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local Notify_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\ExecuteScript | TestData\TestData.csv |

Scenario Outline: Given User is trying to scan SSN for User Action resolution
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify user action details

	Examples:
		| PlayBookName    | PlayBookDescription                   | LeftDecision   | RightDecision | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                      | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                    | TestFile              |
		| UserAction_AUID | Do not delete or update this PlayBook | Classification | User Action   | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local UserAction_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\UserAction | TestData\TestData.csv |

Scenario Outline: Given User is trying to scan SSN for Assign resolution Role
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify assign details

	Examples:
		| PlayBookName    | PlayBookDescription                   | LeftDecision   | RightDecision | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                      | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                    | TestFile              |
		| AssignRole_AUID | Do not delete or update this PlayBook | Classification | Assign_Role   | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local AssignRole_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\AssignRole | TestData\TestData.csv |

Scenario Outline: Given User is trying to scan SSN for Assign resolution User
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify assign details

	Examples:
		| PlayBookName    | PlayBookDescription                   | LeftDecision   | RightDecision | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                      | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                    | TestFile              |
		| AssignUser_AUID | Do not delete or update this PlayBook | Classification | Assign_User   | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local AssignUser_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\AssignUser | TestData\TestData.csv |

Scenario Outline: Given User is trying to scan SSN for Take No Action resolution
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details 'No Action Taken'

	Examples:
		| PlayBookName      | PlayBookDescription                   | LeftDecision   | RightDecision  | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                        | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                      | TestFile              |
		| TakeNoAction_AUID | Do not delete or update this PlayBook | Classification | Take No Action | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local TakeNoAction_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\TakeNoAction | TestData\TestData.csv |

@ignore
Scenario Outline: Given User is trying to scan SSN for Ignore resolution
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details '<RightDecision>'

	Examples:
		| PlayBookName | PlayBookDescription                   | LeftDecision   | RightDecision | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                  | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                | TestFile              |
		| Ignore_AUID  | Do not delete or update this PlayBook | Classification | Ignore        | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local Ignore_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\Ignore | TestData\TestData.csv |

Scenario Outline: Given User is trying to test Restrict Access Scan Functionality for administrator
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details 'Access Restricted'
	Then Administrator is able to access the file '<FolderPath>'
	Then Login as RestrictedUser and file should not be accessible '<FolderPath>'

	Examples:
		| PlayBookName              | PlayBookDescription                   | LeftDecision   | RightDecision                 | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                                | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                                              | TestFile              |
		| RestrictAccess_Admin_AUID | Do not delete or update this PlayBook | Classification | Restrict Access_Administrator | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local RestrictAccess_Admin_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\RestrictAccessOnlyFileAdmministrator | TestData\TestData.csv |

Scenario Outline: Given User is trying to test Restrict Access Scan Functionality for file owner administrator
	Given User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details 'Access Restricted'
	Then Administrator is able to access the file '<FolderPath>'
	Then Login as RestrictedUser and file should not be accessible '<FolderPath>'

	Examples:
		| PlayBookName                  | PlayBookDescription                   | LeftDecision   | RightDecision             | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                              | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                                              | TestFile              |
		| RestrictAccess_FileOwner_AUID | Do not delete or update this PlayBook | Classification | Restrict Access_FileOwner | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local RA_FileOwner_Admin_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\RestrictAccessOnlyFileAdmministrator | TestData\TestData.csv |

@Ignore
Scenario Outline: Given User is trying to test Restrict Access Scan Functionality for file owner restricted user
	Given Restricted User is preparing testData at '<ScanTargetType>' for 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details 'Access Restricted'
	Then Administrator is not able to access the file '<FolderPath>'
	Then Login as RestrictedUser and file should be accessible '<FolderPath>'

	Examples:
		| PlayBookName                  | PlayBookDescription                   | LeftDecision   | RightDecision             | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                           | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath                                     | TestFile              |
		| RestrictAccess_FileOwner_AUID | Do not delete or update this PlayBook | Classification | Restrict Access_FileOwner | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD Local RA_FileOwner_RU_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \SensitiveDataScan\RestrictAccessOnlyFileOwner | TestData\TestData.csv |