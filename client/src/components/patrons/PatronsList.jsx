import { useEffect, useState } from "react";
import { Table, Button } from "reactstrap";
import { getPatrons, deactivatePatron } from "../../data/patronsData";
import { Link } from "react-router-dom";

export default function PatronsList() {
  const [patrons, setPatrons] = useState([]);

  const fetchPatrons = () => {
    getPatrons().then(setPatrons);
  };

  useEffect(() => {
    fetchPatrons();
  }, []);

  const handleDeactivate = (id) => {
    deactivatePatron(id).then(() => {
      fetchPatrons();
    });
  };

  return (
    <div className="container">
      <div className="sub-menu bg-light">
        <h4>Patrons</h4>
      </div>
      <Table>
        <thead>
          <tr>
            <th>Id</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Address</th>
            <th>Email</th>
            <th>Active</th>
            <th></th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {patrons.map((p) => (
            <tr key={`patron-${p.id}`}>
              <th scope="row">{p.id}</th>
              <td>{p.firstName}</td>
              <td>{p.lastName}</td>
              <td>{p.address}</td>
              <td>{p.email}</td>
              <td>{p.isActive ? "Yes" : "No"}</td>
              <td>
                <Link to={`${p.id}`}>Details</Link>
              </td>
              <td>
                {p.isActive && (
                  <Button
                    color="danger"
                    size="sm"
                    onClick={() => handleDeactivate(p.id)}
                  >
                    Deactivate
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
