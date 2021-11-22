Feature: Scan_Cloud

Background:
	Given User is logged in
	Then User is waiting for the dashboard to load
	Then User add a New Dropbox Cloud target agent
		| TargetName                      | TargetType |
		| Cloud_dropbox Target_Automation | Cloud      |

Scenario Outline: Given User is trying to scan SSN for Dropbox
	Given User is preparing testData for Dropbox 'SocialSecurityNumber' from '<TestFile>'  and uploading to '<FolderPath>', 'true'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User finds the created playbook or create new one '<PlayBookName>','<PlayBookDescription>','<LogicName>', '<LogicType>','<LogicTypeOption>','<LeftDecision>' ,'<RightDecision>' and '<ScanItem>'
	Then User creates a new SSD scan with '<ScanName>', '<ScanDescription>', '<PlayBookName>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all details for Multiple Classification '<RightDecision>'

	Examples:
		| PlayBookName      | PlayBookDescription                   | LeftDecision   | RightDecision  | LogicName        | LogicType  | LogicTypeOption | ScanItem               | ScanName                  | ScanDescription                      | ScanType      | ScanTargetType | ScanLocationType   | FolderPath                    | TestFile              |
		| CloudDropBox_AUID | Do not delete or update this PlayBook | Take No Action | Classification | Automation Logic | Data Types | Contains        | Social Security Number | SDM SD CloudDropbox_AUID: | This scan is developed by automation | Cloud;Dropbox | Local Target   | Custom Folder List | \SensitiveDataScan\AssignRole | TestData\TestData.csv |