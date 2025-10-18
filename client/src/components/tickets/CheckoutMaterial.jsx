import { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Form, FormGroup, Label, Input, Button } from "reactstrap";
import { createCheckout } from "../../data/checkoutsData";

export default function CheckoutMaterial() {
  const { materialId } = useParams();
  const navigate = useNavigate();
  const [patronId, setPatronId] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    const checkout = {
      materialId: parseInt(materialId),
      patronId: parseInt(patronId),
      checkoutDate: new Date().toISOString(),
    };
    createCheckout(checkout).then(() => {
      navigate("/checkouts");
    });
  };

  return (
    <div className="container">
      <h4>Checkout Material</h4>
      <Form onSubmit={handleSubmit}>
        <FormGroup>
          <Label for="patronId">Patron ID</Label>
          <Input
            type="number"
            name="patronId"
            id="patronId"
            value={patronId}
            onChange={(e) => setPatronId(e.target.value)}
            required
          />
        </FormGroup>
        <Button color="primary" type="submit">
          Checkout
        </Button>
        <Button
          color="secondary"
          onClick={() => navigate("/browse")}
          className="ms-2"
        >
          Cancel
        </Button>
      </Form>
    </div>
  );
}
