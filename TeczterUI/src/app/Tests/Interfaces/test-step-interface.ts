export interface ITestStep {
    id: number;
    isDeleted: boolean;
    createdOn: string;
    createdById: number;
    revisedOn: string;
    revisedById: number;
    testId: number;
    stepPlacement: number;
    instructions: string;
    linkUrls: string[];
}