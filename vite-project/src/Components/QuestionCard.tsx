import Card from 'react-bootstrap/Card';

function QuestionCard(title: string, text: string) {
  return (
    <Card>
      <Card.Body>
        <Card.Title>{title}</Card.Title>
        <Card.Text>
        {text}
        </Card.Text>
      </Card.Body>
    </Card>
  );
}

export default QuestionCard;