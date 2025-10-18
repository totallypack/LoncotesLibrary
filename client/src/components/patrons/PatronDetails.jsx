import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { Table, Button } from "reactstrap";
import { getPatron } from "../../data/patronsData";

export default function PatronDetails() {
  const { id } = useParams();
  const [patron, setPatron] = useState(null);

  useEffect(() => {
    getPatron(id).then(setPatron);
  }, [id]);

  if (!patron) {
    return <p>Loading...</p>;
  }

  return (
    <div className="container">
      <div className="sub-menu bg-light">
        <h4>
          {patron.firstName} {patron.lastName}
        </h4>
        <Link to={`/patrons/${id}/edit`}>
          <Button color="primary" size="sm">
            Edit
          </Button>
        </Link>
      </div>
      <div>
        <p>
          <strong>Email:</strong> {patron.email}
        </p>
        <p>
          <strong>Address:</strong> {patron.address}
        </p>
        <p>
          <strong>Active:</strong> {patron.isActive ? "Yes" : "No"}
        </p>
        {patron.balance > 0 && (
          <p>
            <strong>Late Fees:</strong> ${patron.balance.toFixed(2)}
          </p>
        )}
      </div>
      <h5>Checkouts</h5>
      {patron.checkouts && patron.checkouts.length > 0 ? (
        <Table>
          <thead>
            <tr>
              <th>Material</th>
              <th>Checkout Date</th>
              <th>Return Date</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            {patron.checkouts.map((c) => (
              <tr key={`checkout-${c.id}`}>
                <td>{c.material?.materialName || "N/A"}</td>
                <td>{new Date(c.checkoutDate).toLocaleDateString()}</td>
                <td>
                  {c.returnDate
                    ? new Date(c.returnDate).toLocaleDateString()
                    : "Not returned"}
                </td>
                <td>{c.returnDate ? "Returned" : "Checked Out"}</td>
              </tr>
            ))}
          </tbody>
        </Table>
      ) : (
        <p>No checkouts found.</p>
      )}
    </div>
  );
}
