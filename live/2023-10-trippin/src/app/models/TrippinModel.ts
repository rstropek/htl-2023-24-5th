export enum PersonGender {
    Male = "Male",
    Female = "Female",
    Unknown = "Unknown"
}

export enum Feature {
    Feature1 = "Feature1",
    Feature2 = "Feature2",
    Feature3 = "Feature3",
    Feature4 = "Feature4"
}

export interface Person {
    /**
     * **Key Property**: This is a key property used to identify the entity.<br/>**Managed**: This property is managed on the server side and cannot be edited.
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `UserName` |
     * | Type | `Edm.String` |
     * | Nullable | `false` |
     */
    UserName: string;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `FirstName` |
     * | Type | `Edm.String` |
     * | Nullable | `false` |
     */
    FirstName: string;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `LastName` |
     * | Type | `Edm.String` |
     */
    LastName: string | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `MiddleName` |
     * | Type | `Edm.String` |
     */
    MiddleName: string | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Gender` |
     * | Type | `Trippin.PersonGender` |
     * | Nullable | `false` |
     */
    Gender: PersonGender;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Age` |
     * | Type | `Edm.Int64` |
     */
    Age: number | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Emails` |
     * | Type | `Collection(Edm.String)` |
     */
    Emails: Array<string>;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `AddressInfo` |
     * | Type | `Collection(Trippin.Location)` |
     */
    AddressInfo: Array<Location>;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `HomeAddress` |
     * | Type | `Trippin.Location` |
     */
    HomeAddress: Location | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `FavoriteFeature` |
     * | Type | `Trippin.Feature` |
     * | Nullable | `false` |
     */
    FavoriteFeature: Feature;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Features` |
     * | Type | `Collection(Trippin.Feature)` |
     * | Nullable | `false` |
     */
    Features: Array<Feature>;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Friends` |
     * | Type | `Collection(Trippin.Person)` |
     */
    Friends?: Array<Person>;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `BestFriend` |
     * | Type | `Trippin.Person` |
     */
    BestFriend?: Person | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Trips` |
     * | Type | `Collection(Trippin.Trip)` |
     */
    Trips?: Array<Trip>;
}

export type PersonId = string | {UserName: string};

export interface EditablePerson extends Pick<Person, "FirstName" | "Gender" | "FavoriteFeature" | "Features">, Partial<Pick<Person, "LastName" | "MiddleName" | "Age" | "Emails">> {
    AddressInfo?: Array<EditableLocation>;
    HomeAddress?: EditableLocation | null;
}

export interface Person_GetFriendsTripsParams {
    userName: string;
}

export interface Person_UpdateLastNameParams {
    lastName: string;
}

export interface Person_ShareTripParams {
    userName: string;
    tripId: number;
}

export interface Airline {
    /**
     * **Key Property**: This is a key property used to identify the entity.<br/>**Managed**: This property is managed on the server side and cannot be edited.
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `AirlineCode` |
     * | Type | `Edm.String` |
     * | Nullable | `false` |
     */
    AirlineCode: string;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Name` |
     * | Type | `Edm.String` |
     */
    Name: string | null;
}

export type AirlineId = string | {AirlineCode: string};

export interface EditableAirline extends Partial<Pick<Airline, "Name">> {
}

export interface Airport {
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Name` |
     * | Type | `Edm.String` |
     */
    Name: string | null;
    /**
     * **Key Property**: This is a key property used to identify the entity.<br/>**Managed**: This property is managed on the server side and cannot be edited.
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `IcaoCode` |
     * | Type | `Edm.String` |
     * | Nullable | `false` |
     */
    IcaoCode: string;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `IataCode` |
     * | Type | `Edm.String` |
     */
    IataCode: string | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Location` |
     * | Type | `Trippin.AirportLocation` |
     */
    Location: AirportLocation | null;
}

export type AirportId = string | {IcaoCode: string};

export interface EditableAirport extends Partial<Pick<Airport, "Name" | "IataCode">> {
    Location?: EditableAirportLocation | null;
}

export interface Trip {
    /**
     * **Key Property**: This is a key property used to identify the entity.<br/>**Managed**: This property is managed on the server side and cannot be edited.
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `TripId` |
     * | Type | `Edm.Int32` |
     * | Nullable | `false` |
     */
    TripId: number;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `ShareId` |
     * | Type | `Edm.Guid` |
     * | Nullable | `false` |
     */
    ShareId: string;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Name` |
     * | Type | `Edm.String` |
     */
    Name: string | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Budget` |
     * | Type | `Edm.Single` |
     * | Nullable | `false` |
     */
    Budget: number;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Description` |
     * | Type | `Edm.String` |
     */
    Description: string | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Tags` |
     * | Type | `Collection(Edm.String)` |
     */
    Tags: Array<string>;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `StartsAt` |
     * | Type | `Edm.DateTimeOffset` |
     * | Nullable | `false` |
     */
    StartsAt: string;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `EndsAt` |
     * | Type | `Edm.DateTimeOffset` |
     * | Nullable | `false` |
     */
    EndsAt: string;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `PlanItems` |
     * | Type | `Collection(Trippin.PlanItem)` |
     */
    PlanItems?: Array<PlanItem>;
}

export type TripId = number | {TripId: number};

export interface EditableTrip extends Pick<Trip, "ShareId" | "Budget" | "StartsAt" | "EndsAt">, Partial<Pick<Trip, "Name" | "Description" | "Tags">> {
}

export interface PlanItem {
    /**
     * **Key Property**: This is a key property used to identify the entity.<br/>**Managed**: This property is managed on the server side and cannot be edited.
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `PlanItemId` |
     * | Type | `Edm.Int32` |
     * | Nullable | `false` |
     */
    PlanItemId: number;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `ConfirmationCode` |
     * | Type | `Edm.String` |
     */
    ConfirmationCode: string | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `StartsAt` |
     * | Type | `Edm.DateTimeOffset` |
     * | Nullable | `false` |
     */
    StartsAt: string;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `EndsAt` |
     * | Type | `Edm.DateTimeOffset` |
     * | Nullable | `false` |
     */
    EndsAt: string;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Duration` |
     * | Type | `Edm.Duration` |
     * | Nullable | `false` |
     */
    Duration: string;
}

export type PlanItemId = number | {PlanItemId: number};

export interface EditablePlanItem extends Pick<PlanItem, "StartsAt" | "EndsAt" | "Duration">, Partial<Pick<PlanItem, "ConfirmationCode">> {
}

export interface Event extends PlanItem {
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `OccursAt` |
     * | Type | `Trippin.EventLocation` |
     */
    OccursAt: EventLocation | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Description` |
     * | Type | `Edm.String` |
     */
    Description: string | null;
}

export interface EditableEvent extends Pick<Event, "StartsAt" | "EndsAt" | "Duration">, Partial<Pick<Event, "ConfirmationCode" | "Description">> {
    OccursAt?: EditableEventLocation | null;
}

export interface PublicTransportation extends PlanItem {
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `SeatNumber` |
     * | Type | `Edm.String` |
     */
    SeatNumber: string | null;
}

export interface EditablePublicTransportation extends Pick<PublicTransportation, "StartsAt" | "EndsAt" | "Duration">, Partial<Pick<PublicTransportation, "ConfirmationCode" | "SeatNumber">> {
}

export interface Flight extends PublicTransportation {
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `FlightNumber` |
     * | Type | `Edm.String` |
     */
    FlightNumber: string | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Airline` |
     * | Type | `Trippin.Airline` |
     */
    Airline?: Airline | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `From` |
     * | Type | `Trippin.Airport` |
     */
    From?: Airport | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `To` |
     * | Type | `Trippin.Airport` |
     */
    To?: Airport | null;
}

export interface EditableFlight extends Pick<Flight, "StartsAt" | "EndsAt" | "Duration">, Partial<Pick<Flight, "ConfirmationCode" | "SeatNumber" | "FlightNumber">> {
}

export interface Employee extends Person {
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Cost` |
     * | Type | `Edm.Int64` |
     * | Nullable | `false` |
     */
    Cost: number;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Peers` |
     * | Type | `Collection(Trippin.Person)` |
     */
    Peers?: Array<Person>;
}

export interface EditableEmployee extends Pick<Employee, "FirstName" | "Gender" | "FavoriteFeature" | "Features" | "Cost">, Partial<Pick<Employee, "LastName" | "MiddleName" | "Age" | "Emails">> {
    AddressInfo?: Array<EditableLocation>;
    HomeAddress?: EditableLocation | null;
}

export interface Manager extends Person {
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Budget` |
     * | Type | `Edm.Int64` |
     * | Nullable | `false` |
     */
    Budget: number;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `BossOffice` |
     * | Type | `Trippin.Location` |
     */
    BossOffice: Location | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `DirectReports` |
     * | Type | `Collection(Trippin.Person)` |
     */
    DirectReports?: Array<Person>;
}

export interface EditableManager extends Pick<Manager, "FirstName" | "Gender" | "FavoriteFeature" | "Features" | "Budget">, Partial<Pick<Manager, "LastName" | "MiddleName" | "Age" | "Emails">> {
    AddressInfo?: Array<EditableLocation>;
    HomeAddress?: EditableLocation | null;
    BossOffice?: EditableLocation | null;
}

export interface Location {
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Address` |
     * | Type | `Edm.String` |
     */
    Address: string | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `City` |
     * | Type | `Trippin.City` |
     */
    City: City | null;
}

export interface EditableLocation extends Partial<Pick<Location, "Address">> {
    City?: EditableCity | null;
}

export interface City {
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Name` |
     * | Type | `Edm.String` |
     */
    Name: string | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `CountryRegion` |
     * | Type | `Edm.String` |
     */
    CountryRegion: string | null;
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Region` |
     * | Type | `Edm.String` |
     */
    Region: string | null;
}

export interface EditableCity extends Partial<Pick<City, "Name" | "CountryRegion" | "Region">> {
}

export interface AirportLocation extends Location {
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `Loc` |
     * | Type | `Edm.GeographyPoint` |
     */
    Loc: string | null;
}

export interface EditableAirportLocation extends Partial<Pick<AirportLocation, "Address" | "Loc">> {
    City?: EditableCity | null;
}

export interface EventLocation extends Location {
    /**
     *
     * OData Attributes:
     * |Attribute Name | Attribute Value |
     * | --- | ---|
     * | Name | `BuildingInfo` |
     * | Type | `Edm.String` |
     */
    BuildingInfo: string | null;
}

export interface EditableEventLocation extends Partial<Pick<EventLocation, "Address" | "BuildingInfo">> {
    City?: EditableCity | null;
}

export interface GetNearestAirportParams {
    lat: number;
    lon: number;
}
