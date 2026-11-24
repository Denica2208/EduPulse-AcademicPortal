import { useState } from 'react';
import { SubmissionModal } from './components/SubmissionModal';
import { GradingPanel } from './components/GradingPanel';

export function App() {
  const [role, setRole] = useState<'Student' | 'Professor'>('Student');

  return (
    <div style={{ fontFamily: 'Segoe UI, sans-serif', maxWidth: 960, margin: '0 auto', padding: 24 }}>
      <header style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', borderBottom: '2px solid #1B365D', paddingBottom: 12, marginBottom: 20 }}>
        <div>
          <h1 style={{ margin: 0, color: '#1B365D' }}>EduPulse Academic Portal</h1>
          <small style={{ color: '#4A5568' }}>Course & Laboratory Management Prototype</small>
        </div>
        <div>
          <button onClick={() => setRole('Student')} style={{ marginRight: 8, fontWeight: role === 'Student' ? 'bold' : 'normal' }}>
            Student View
          </button>
          <button onClick={() => setRole('Professor')} style={{ fontWeight: role === 'Professor' ? 'bold' : 'normal' }}>
            Professor View
          </button>
        </div>
      </header>

      {role === 'Student' ? <SubmissionModal /> : <GradingPanel />}
    </div>
  );
}

export default App;
