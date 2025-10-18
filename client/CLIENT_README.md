# Loncotes County Library - React Client

This is the React frontend for the Loncotes County Library management system.

## Setup

1. Make sure you have Node.js installed
2. Navigate to the client directory: `cd client`
3. Install dependencies: `npm install`
4. The API is configured to run on port 5001 (HTTPS). Make sure your .NET API is configured to use this port.

## Running the Application

1. Start the .NET API from the root directory:
   ```bash
   cd ..
   dotnet run
   ```

2. In a separate terminal, start the React development server:
   ```bash
   cd client
   npm run dev
   ```

3. Open your browser to the URL shown in the terminal (usually http://localhost:5173)

## Features Implemented

### Materials Management
- **Materials List** (`/materials`) - View all circulating materials
- **Material Details** (`/materials/:id`) - View detailed information about a material
- **Create Material** (`/materials/create`) - Add a new material to the library
- **Remove from Circulation** - Button in materials list to remove items
- **Browse Available Materials** (`/browse`) - View only materials available for checkout

### Patron Management
- **Patrons List** (`/patrons`) - View all patrons with their active status
- **Patron Details** (`/patrons/:id`) - View patron information including late fees and checkout history
- **Edit Patron** (`/patrons/:id/edit`) - Update patron's address and email
- **Deactivate Patron** - Button to deactivate active patrons (soft delete)

### Checkout Management
- **Checkouts List** (`/checkouts`) - View all checkouts with materials and patrons
- **Return Item** - Button to return currently checked out items
- **Checkout Material** (`/materials/:materialId/checkout`) - Form to checkout a material to a patron
- **Overdue Checkouts** (`/checkouts/overdue`) - View all overdue checkouts with patron information

## Project Structure

```
client/
├── src/
│   ├── components/
│   │   ├── checkouts/       # Checkout-related components
│   │   │   ├── CheckoutsList.jsx
│   │   │   └── OverdueCheckouts.jsx
│   │   ├── patrons/         # Patron-related components
│   │   │   ├── PatronsList.jsx
│   │   │   ├── PatronDetails.jsx
│   │   │   └── PatronEdit.jsx
│   │   └── tickets/         # Material-related components
│   │       ├── MaterialList.jsx
│   │       ├── MaterialDetails.jsx
│   │       ├── CreateMaterial.jsx
│   │       ├── Browse.jsx
│   │       └── CheckoutMaterial.jsx
│   ├── data/                # API service layer
│   │   ├── materialsData.js
│   │   ├── patronsData.js
│   │   ├── checkoutsData.js
│   │   ├── genresData.js
│   │   └── materialTypesData.js
│   ├── App.jsx              # Main app with navigation
│   └── index.jsx            # Routes configuration
├── package.json
└── vite.config.js
```

## API Endpoints Used

### Materials
- `GET /api/materials` - Get all circulating materials
- `GET /api/materials/:id` - Get material by ID
- `POST /api/materials` - Create new material
- `PUT /api/materials/:id/remove` - Remove material from circulation
- `GET /api/materials/available` - Get available materials

### Patrons
- `GET /api/patrons` - Get all patrons
- `GET /api/patrons/:id` - Get patron by ID with checkout history
- `PUT /api/patrons/:id` - Update patron
- `PUT /api/patrons/:id/deactivate` - Deactivate patron

### Checkouts
- `GET /api/checkouts` - Get all checkouts
- `POST /api/checkouts` - Create new checkout
- `PUT /api/checkouts/:id/return` - Return a checkout
- `GET /api/checkouts/overdue` - Get overdue checkouts

### Reference Data
- `GET /api/genres` - Get all genres
- `GET /api/materialtypes` - Get all material types

## Technologies Used

- React 18
- React Router DOM 6
- Reactstrap (Bootstrap 5 components)
- Vite (build tool)
