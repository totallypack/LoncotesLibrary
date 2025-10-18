import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Form, FormGroup, Label, Input, Button } from "reactstrap";
import { getPatron, updatePatron } from "../../data/patronsData";

export default function PatronEdit() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [patron, setPatron] = useState({
    address: "",
    email: "",
  });

  useEffect(() => {
    getPatron(id).then((data) => {
      setPatron({
        address: data.address,
        email: data.email,
      });
    });
  }, [id]);

  const handleSubmit = (e) => {
    e.preventDefault();
    updatePatron(id, patron).then(() => {
      navigate(`/patrons/${id}`);
    });
  };

  return (
    <div className="container">
      <h4>Edit Patron</h4>
      <Form onSubmit={handleSubmit}>
        <FormGroup>
          <Label for="address">Address</Label>
          <Input
            type="text"
            name="address"
            id="address"
            value={patron.address}
            onChange={(e) =>
              setPatron({ ...patron, address: e.target.value })
            }
          />
        </FormGroup>
        <FormGroup>
          <Label for="email">Email</Label>
          <Input
            type="email"
            name="email"
            id="email"
            value={patron.email}
            onChange={(e) =>
              setPatron({ ...patron, email: e.target.value })
            }
          />
        </FormGroup>
        <Button color="primary" type="submit">
          Save
        </Button>
        <Button
          color="secondary"
          onClick={() => navigate(`/patrons/${id}`)}
          className="ms-2"
        >
          Cancel
        </Button>
      </Form>
    </div>
  );
}
