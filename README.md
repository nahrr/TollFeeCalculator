# **Toll Fee Calculator**

The Toll Fee Calculator is a backend application built with .NET 8. It calculates toll fees for vehicles based on time, vehicle type, and other rules such as daily maximum fees and exemptions for toll-free dates.

## **Table of Contents**
1. [Getting Started](#getting-started)
2. [API Endpoints](#api-endpoints)
3. [Features](#features)
4. [Improvements](#improvements)
5. [What's Missing?](#whats-missing)

---

## **Getting Started**

### **1. Prerequisites**
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### **2. Setup**

   ```bash
   git clone <repository-url>
   cd TollFeeCalculator
   dotnet build
   dotnet run
   ```
### API Endpoints

#### GET /toll-fee
 **Example request** 
   ```bash
curl -X 'GET' \
  'https://localhost:5000/toll-fee?VehicleType=car&TollPassTimes=2025-01-05T17%3A43%3A45.258Z' \
  -H 'accept: application/json'
 ```

## **Features**

### **Time-Based Toll Rules**
- Different fees based on the time of day.
- Daily maximum toll cap of 60 SEK.

### **Toll-Free Conditions**
- Toll-free dates (weekends, public holidays).
- Toll-free vehicles (e.g., emergency vehicles, motorbikes).

### **Caching**
- Implements caching for holiday data to improve performance.

### **Unit Tests**
- Includes unit tests for core logic.

---

## **Improvements**
1. **Static Endpoint Module**:
    - Difficult to unit test due to static design.

2. **Hardcoded Data**:
    - `MockHolidayApi` provides static holiday data. Could potentially use an EF Core in-memory database for better extensibility.

3. **Minimal Error Handling**:
    - Limited structured error responses for invalid inputs.

4. **No Domain Layer**:
    - Business logic could be better separated into a dedicated domain layer.

5. **Logging**
   - Add structured and centralized logging to improve debugging and observability.
