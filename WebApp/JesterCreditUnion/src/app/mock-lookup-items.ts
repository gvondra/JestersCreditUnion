import { LookupItem } from './lookup-item';
export const LOOKUP_ITEMS: any = {
    "Gender": [
        {"Group": "Gender", "Code": "F", "Description": "Female"},
        {"Group": "Gender", "Code": "M", "Description": "Male"},
        {"Group": "Gender", "Code": "O", "Description": "Other"}
    ],
    "OccupancyStatus": [
        {"Group": "OccupancyStatus", "Code": "OM", "Description": "Own home + Mortage"},
        {"Group": "OccupancyStatus", "Code": "WPARENTS", "Description": "Live with parents"},
        {"Group": "OccupancyStatus", "Code": "OWN", "Description": "Own home + Free & Clear"},
        {"Group": "OccupancyStatus", "Code": "RENT", "Description": "Rent"},
        {"Group": "OccupancyStatus", "Code": "GOVQUARTERS", "Description": "Government Quarters"},
        {"Group": "OccupancyStatus", "Code": "O", "Description": "Other"}
    ],
    "EmploymentStatus": [
        {"Group": "EmploymentStatus", "Code": "E", "Description": "Employed"},
        {"Group": "EmploymentStatus", "Code": "R", "Description": "Retired"},
        {"Group": "EmploymentStatus", "Code": "SE", "Description": "Self Employed"},
        {"Group": "EmploymentStatus", "Code": "ST", "Description": "Student"},
        {"Group": "EmploymentStatus", "Code": "U", "Description": "Unemployed"},
        {"Group": "EmploymentStatus", "Code": "O", "Description": "Other"}
    ]
}