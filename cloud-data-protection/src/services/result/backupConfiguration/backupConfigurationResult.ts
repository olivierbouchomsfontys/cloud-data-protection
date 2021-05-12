import {BackupFrequency} from "entities/backupFrequency";

interface BackupConfigurationResult {
    id: number;
    createdAt: Date;
    frequency: BackupFrequency;
    hour: number;
    minute: number;
    description: string;
}

export default BackupConfigurationResult;