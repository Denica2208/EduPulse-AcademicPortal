import { useState } from 'react';

interface StudentSubmission {
  id: string;
  studentName: string;
  group: string;
  submittedAt: string;
  score: number | null;
  feedback: string;
}

const initialSubmissions: StudentSubmission[] = [
  { id: '1', studentName: 'Preduț Andreea', group: 'CEN 4.S2 B', submittedAt: 'Nov 24, 16:30', score: 10.0, feedback: 'Clean architecture and complete tests.' },
  { id: '2', studentName: 'Dincă Alexia', group: 'CEN 4.S2 B', submittedAt: 'Nov 24, 15:10', score: 9.5, feedback: 'Well structured endpoints.' },
];

export function GradingPanel() {
  const [submissions, setSubmissions] = useState<StudentSubmission[]>(initialSubmissions);
  const [selectedId, setSelectedId] = useState<string>('1');
  const [score, setScore] = useState<string>('10.0');
  const [feedback, setFeedback] = useState<string>('Clean architecture and complete tests.');

  const handleSave = () => {
    const parsedScore = parseFloat(score);
    if (isNaN(parsedScore) || parsedScore < 1 || parsedScore > 10) return;

    setSubmissions((prev) =>
      prev.map((item) =>
        item.id === selectedId ? { ...item, score: parsedScore, feedback } : item
      )
    );
  };

  return (
    <section style={{ border: '1px solid #CBD5E0', borderRadius: 8, padding: 16 }}>
      <h2 style={{ marginTop: 0, color: '#1B365D' }}>Professor Evaluation Panel (Group: CEN 4.S2 B)</h2>
      <table style={{ width: '100%', borderCollapse: 'collapse', marginBottom: 16 }}>
        <thead>
          <tr style={{ backgroundColor: '#EDF2F7', textAlign: 'left' }}>
            <th style={{ padding: 6 }}>Student</th>
            <th style={{ padding: 6 }}>Submitted</th>
            <th style={{ padding: 6 }}>Grade</th>
          </tr>
        </thead>
        <tbody>
          {submissions.map((s) => (
            <tr
              key={s.id}
              onClick={() => {
                setSelectedId(s.id);
                setScore(s.score ? s.score.toString() : '');
                setFeedback(s.feedback);
              }}
              style={{ cursor: 'pointer', backgroundColor: s.id === selectedId ? '#EBF8FF' : 'transparent' }}
            >
              <td style={{ padding: 6 }}>{s.studentName}</td>
              <td style={{ padding: 6 }}>{s.submittedAt}</td>
              <td style={{ padding: 6 }}>{s.score ?? 'Pending'}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <div>
        <label>
          Grade (1.0 – 10.0):{' '}
          <input value={score} onChange={(e) => setScore(e.target.value)} style={{ width: 60 }} />
        </label>
      </div>
      <div style={{ marginTop: 8 }}>
        <label>
          Feedback:
          <br />
          <textarea value={feedback} onChange={(e) => setFeedback(e.target.value)} rows={3} style={{ width: '100%' }} />
        </label>
      </div>
      <button onClick={handleSave} style={{ marginTop: 8 }}>Save Grade</button>
    </section>
  );
}
