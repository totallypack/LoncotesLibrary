import { useEffect, useState } from "react";
import { Table } from "reactstrap";
import { getOverdueCheckouts } from "../../data/checkoutsData";

export default function OverdueCheckouts() {
  const [checkouts, setCheckouts] = useState([]);

  useEffect(() => {
    getOverdueCheckouts().then(setCheckouts);
  }, []);

  return (
    <div className="container">
      <div className="sub-menu bg-light">
        <h4>Overdue Checkouts</h4>
      </div>
      <Table>
        <thead>
          <tr>
            <th>Id</th>
            <th>Material</th>
            <th>Patron</th>
            <th>Checkout Date</th>
            <th>Days Overdue</th>
          </tr>
        </thead>
        <tbody>
          {checkouts.map((c) => {
            const checkoutDate = new Date(c.checkoutDate);
            const dueDate = new Date(checkoutDate);
            dueDate.setDate(
              dueDate.getDate() + c.material.materialType.checkoutDays
            );
            const today = new Date();
            const daysOverdue = Math.floor(
              (today - dueDate) / (1000 * 60 * 60 * 24)
            );

            return (
              <tr key={`checkout-${c.id}`}>
                <th scope="row">{c.id}</th>
                <td>{c.material?.materialName || "N/A"}</td>
                <td>
                  {c.patron
                    ? `${c.patron.firstName} ${c.patron.lastName}`
                    : "N/A"}
                </td>
                <td>{checkoutDate.toLocaleDateString()}</td>
                <td>{daysOverdue} days</td>
              </tr>
            );
          })}
        </tbody>
      </Table>
    </div>
  );
}
