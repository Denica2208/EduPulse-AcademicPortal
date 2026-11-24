import { useState } from 'react';

export function SubmissionModal() {
  const [selectedFile, setSelectedFile] = useState<string>('Lab3_Implementation.zip');
  const [status, setStatus] = useState<string>('Ready to upload');

  const handleUpload = () => {
    setStatus(`Uploaded ${selectedFile} at ${new Date().toLocaleTimeString()} (On Time)`);
  };

  return (
    <section style={{ border: '1px solid #CBD5E0', borderRadius: 8, padding: 16 }}>
      <h2 style={{ marginTop: 0, color: '#1B365D' }}>Lab 3: REST API & Database Implementation</h2>
      <p>Deadline: Nov 27, 2026, 23:59 | Allowed formats: <code>.zip</code>, <code>.rar</code> (max 25 MB)</p>

      <input
        type="file"
        accept=".zip,.rar"
        onChange={(e) => {
          const file = e.target.files?.[0];
          if (file) setSelectedFile(file.name);
        }}
      />
      <div style={{ marginTop: 12 }}>
        <button onClick={handleUpload}>Upload Solution</button>
      </div>
      <p style={{ marginTop: 12, color: '#16A34A', fontWeight: 600 }}>Status: {status}</p>
    </section>
  );
}
