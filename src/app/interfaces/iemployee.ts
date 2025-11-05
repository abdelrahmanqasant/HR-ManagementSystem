export interface IEmployee {
  id: number;
  ssn: string;
  fullName: string;
  address: string;
  phoneNumber: string;
  gender: number;
  nationality: string;
  birthDate: string;
  contractDate: string;
  baseSalary: number;
  arrival: string;
  departure: string;
  departmentName: string;
}

export enum Gender {
  Male = 0,
  Female = 1,
}
