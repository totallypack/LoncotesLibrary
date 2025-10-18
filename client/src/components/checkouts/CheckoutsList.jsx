import { useEffect, useState } from "react";
import { Table, Button } from "reactstrap";
import { getCheckouts, returnCheckout } from "../../data/checkoutsData";

export default function CheckoutsList() {
  const [checkouts, setCheckouts] = useState([]);

  const fetchCheckouts = () => {
    getCheckouts().then(setCheckouts);
  };

  useEffect(() => {
    fetchCheckouts();
  }, []);

  const handleReturn = (id) => {
    returnCheckout(id).then(() => {
      fetchCheckouts();
    });
  };

  return (
    <div className="container">
      <div className="sub-menu bg-light">
        <h4>Checkouts</h4>
      </div>
      <Table>
        <thead>
          <tr>
            <th>Id</th>
            <th>Material</th>
            <th>Patron</th>
            <th>Checkout Date</th>
            <th>Return Date</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {checkouts.map((c) => (
            <tr key={`checkout-${c.id}`}>
              <th scope="row">{c.id}</th>
              <td>{c.material?.materialName || "N/A"}</td>
              <td>
                {c.patron
                  ? `${c.patron.firstName} ${c.patron.lastName}`
                  : "N/A"}
              </td>
              <td>{new Date(c.checkoutDate).toLocaleDateString()}</td>
              <td>
                {c.returnDate
                  ? new Date(c.returnDate).toLocaleDateString()
                  : "Not returned"}
              </td>
              <td>
                {!c.returnDate && (
                  <Button
                    color="success"
                    size="sm"
                    onClick={() => handleReturn(c.id)}
                  >
                    Return
                  </Button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
}
