Feature: DiscoveryScan
	Testing Discovery Scan

Background:
	Given User is logged in

Scenario Outline: Given User is trying to scan files using Discovery Scan Local Target
	Then Create directory on Local machine
	Given User is preparing testData for DiscoveryScan from '<TestFile>' for '<ScanTargetType>'  and uploading to '<FolderPath>'
	Then User is waiting for the dashboard to load
	Then User selects to click on new scan
	Then User creates a new Discovery scan with '<ScanName>', '<ScanDescription>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all files are discovered

	Examples:
		| ScanName                     | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath     | TestFile               |
		| SDM DS Local Discovery_AUID: | This scan is developed by automation | Files & Folders | Local Target   | Custom Folder List | \DiscoveryScan | TestData\DiscoveryScan |

Scenario Outline: Given User is trying to scan files using Discovery Scan Remote Target
	Then Create directory on Remote machine
	Given User is preparing testData for DiscoveryScan from '<TestFile>' for '<ScanTargetType>'  and uploading to '<FolderPath>'
	Then User is waiting for the dashboard to load
	Then User add a new remote target agent
		| TargetName               | TargetType | AddressType |
		| Remote Target_Automation | Remote     | IP Address  |
	Then User selects to click on new scan
	Then User creates a new Discovery scan with '<ScanName>', '<ScanDescription>', '<ScanType>','<ScanTargetType>', '<ScanLocationType>', '<FolderPath>'
	Then Run the scan and wait for scan to complete
	And Open Search Scan Results and verify all files are discovered

	Examples:
		| ScanName                      | ScanDescription                      | ScanType        | ScanTargetType | ScanLocationType   | FolderPath     | TestFile               |
		| SDM DS Remote Discovery_AUID: | This scan is developed by automation | Files & Folders | Remote Target  | Custom Folder List | \DiscoveryScan | TestData\DiscoveryScan |