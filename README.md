# Save-IT-Finances

## Project Overview
Save-IT Finances is a financial management application designed to help users manage their savings, investments, and currency conversions. The application also facilitates client management and quote retrieval for financial services staff.

## Technologies and Tools Used
- **Programming Languages**: C#
- **Frameworks**: .NET Framework, Windows Forms
- **Database**: MySQL
- **Web Server**: XAMPP
- **Version Control**: Git, GitHub

## Modules

### 1. Currency Conversion Module

#### Scope Fulfilment:
- **Inputs**: 
  - Users can input the amount, source and target currencies, and client name.

- **Processes**:
  - The module validates data format and range to avoid invalid conversions.
  - The app does not fetch real-time currency rates through an API to ensure up-to-date exchange information because these services are paid.
  - Conversion fees are calculated using a tiered fee structure to reflect accurate transaction costs.

- **Outputs**:
  - Provides the converted amount based on accurate exchange rates.
  - Calculates conversion fees and directly shows the final amount after fees.

### 2. Savings and Investments Module

#### Scope Fulfilment:
- **Inputs**: 
  - Users can enter an initial investment amount, monthly contributions, and investment type.

- **Processes**:
  - Collects data about investment preferences.
  - Projects earnings, fees, and taxes over different periods using accurate calculations tailored to investment type and user preferences.
  - Computes fees and taxes per predefined rules, ensuring accuracy.

- **Outputs**:
  - Displays projected earnings and detailed fee/tax information to help users make informed decisions.

### 3. New Client Module

#### Scope Fulfilment:
- **Input**: 
  - Staff can securely input client details, including full name, email, address, postcode, and phone number.

- **Processes**:
  - Secure data storage in a centralized database, ensuring client data protection.
  - Confirmation message displayed upon successful data entry, providing user feedback. In case of errors, appropriate messages guide staff.

### 4. Retrieve Quotes Module

#### Scope Fulfilment:
- **Processes**:
  - Allows staff to retrieve saved quotes by searching for client names.
  - Client search functionality quickly locates relevant data in the database.
  - Data display ensures previous transactions and advisory details are visible, aiding client service and further financial planning.

## Functional Requirements
1. **FR1**: The system allows users to log in with valid credentials, ensuring only authorized access.
2. **FR2**: Users can choose from a list of supported currencies for conversion.
3. **FR3**: Users can input the amount for conversion, initial and monthly investment amounts, and select an investment plan, facilitating accurate financial planning.
4. **FR4**: The system calculates and displays investment results based on user inputs and the selected investment plan. It also accurately applies and shows fees and taxes for conversions and investments.
5. **FR5**: New clients can be securely added to the database, ensuring efficient client management.
6. **FR6**: The system allows retrieving saved quotes, offering a valuable reference for previous transactions.
7. **FR7**: The system displays transaction limits, fees, and details of savings plans, giving users transparency and informed decision-making.
8. **FR8**: Users can easily navigate between application modules, simplifying the user experience.

## Non-Functional Requirements
1. **NFR1**: Scalability: The system is designed to scale up and support an increasing number of transactions as the client base expands, ensuring smooth operations.
2. **NFR2**: Security: The system complies with the latest security standards to protect sensitive financial data through encryption and access control.
3. **NFR3**: Response Time: The system responds to user requests within 2 seconds for 90% of transactions.
4. **NFR4**: Consistency: The UI lacks a consistent design across all screens.
5. **NFR5**: Colour Scheme: The buttons do not use a colour palette that is easy on the eyes or ensures visibility and accessibility for colour-blind users. However, they are consistent with the overall UI theme.






