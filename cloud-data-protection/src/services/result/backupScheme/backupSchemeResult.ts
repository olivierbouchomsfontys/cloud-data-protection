import {BackupFrequency} from "entities/backupFrequency";

interface BackupSchemeResult {
    id: number;
    frequency: BackupFrequency;
    hour: number;
    minute: number;
    description: string;
}

export default BackupSchemeResult;