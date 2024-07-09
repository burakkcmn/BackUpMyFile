# BackupMyFile

BackupMyFile is a C# application that automates the process of backing up files from a source folder to a destination folder. It supports automatic backups based on a specified time interval or when a file is changed in the source folder.

## Features
- **Automatic Backup:** Automatically backs up files from the source folder to the destination folder.

- **Heartbeat Mechanism:** Regularly updates the status of registered devices (online/offline) in the database.
- **USB Detection:** Detects when USB drives are inserted or removed and adjusts the backup process accordingly.
- **File Change Monitoring:** Monitors files for changes and triggers backups when changes are detected.
- **Configurable Backup Period:** Allows configuration of backup intervals or triggers backups when files change.
- **Logging:** Logs activities and errors for easy monitoring and debugging.


## Getting Started
### Prerequisites
- .NET Framework (version compatible with the application)
- Windows OS

### Installation
1. Clone this repository:
    ```sh
    git clone https://github.com/yourusername/BackupMyFile.git
    ```
2. Open the solution in Visual Studio.

3. Build the solution to restore the necessary NuGet packages and dependencies.

## Usage
1. **Configure the backup settings in the application:**
   - **Source Folder:** The folder you want to back up.
   - **Destination Folder:** The folder where backups will be stored.
   - **Backup Period:** Time interval for automatic backups or set to trigger on file changes.

2. **Run the application.**

3. **The application will monitor the source folder and perform backups based on the configured settings.**

## Contributing
- Fork the repository.
- Create your feature branch (git checkout -b feature/YourFeature).
- Commit your changes (git commit -m 'Add some feature').
- Push to the branch (git push origin feature/YourFeature).
- Open a pull request.

## License
This project is licensed under the BSD 3-Clause License - see the [LICENSE](LICENSE) file for details.

This README provides a comprehensive overview of your project, including setup instructions, usage details, and code structure. Adjust the content as needed to better fit your specific project requirements.

## Author
- **Burak KOCAMAN**
  - GitHub: [burakkcmn](https://github.com/burakkcmn)
  - Email: [kocaman.burak.bk@gmail.com](mailto:kocaman.burak.bk@gmail.com)
