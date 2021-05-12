import {BackupFrequency} from "entities/backupFrequency";

interface CreateBackupConfigurationResult {
    id: number;
    createdAt: Date;
    frequency: BackupFrequency;
    hour: number;
    minute: number;
    description: string;
}

export default CreateBackupConfigurationResult;