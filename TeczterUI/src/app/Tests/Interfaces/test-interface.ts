import { ITestStep } from "./test-step-interface";

export interface ITest {
    id: number,
    isDeleted: boolean,
    createdOn: string,
    createdById: number;
    revisedOn: string;
    title: string;
    description: string;
    linkUrls : string[];
    department: string;
    testSteps: ITestStep[];
}