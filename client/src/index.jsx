import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import MaterialList from "./components/tickets/MaterialList";
import MaterialDetails from "./components/tickets/MaterialDetails";
import CreateMaterial from "./components/tickets/CreateMaterial";
import Browse from "./components/tickets/Browse";
import CheckoutMaterial from "./components/tickets/CheckoutMaterial";
import PatronsList from "./components/patrons/PatronsList";
import PatronDetails from "./components/patrons/PatronDetails";
import PatronEdit from "./components/patrons/PatronEdit";
import CheckoutsList from "./components/checkouts/CheckoutsList";
import OverdueCheckouts from "./components/checkouts/OverdueCheckouts";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <BrowserRouter>
    <Routes>
      <Route path="/" element={<App />}>
        <Route path="materials">
          <Route index element={<MaterialList />} />
          <Route path=":id" element={<MaterialDetails />} />
          <Route path="create" element={<CreateMaterial />} />
          <Route path=":materialId/checkout" element={<CheckoutMaterial />} />
        </Route>
        <Route path="patrons">
          <Route index element={<PatronsList />} />
          <Route path=":id" element={<PatronDetails />} />
          <Route path=":id/edit" element={<PatronEdit />} />
        </Route>
        <Route path="checkouts">
          <Route index element={<CheckoutsList />} />
          <Route path="overdue" element={<OverdueCheckouts />} />
        </Route>
        <Route path="browse" element={<Browse />} />
      </Route>
    </Routes>
  </BrowserRouter>,
);
