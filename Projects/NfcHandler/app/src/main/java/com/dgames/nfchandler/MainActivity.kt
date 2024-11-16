package com.dgames.nfchandler

import android.os.Build
import android.app.PendingIntent
import android.content.Intent
import android.content.IntentFilter
import android.nfc.NfcAdapter
import android.nfc.Tag
import android.os.Bundle
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity

class MainActivity : AppCompatActivity() {

    private var nfcAdapter: NfcAdapter? = null
    private lateinit var tagInfoTextView: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        tagInfoTextView = findViewById(R.id.tag_info)

        // Initialiser le NFC Adapter
        nfcAdapter = NfcAdapter.getDefaultAdapter(this)
        if (nfcAdapter == null) {
            Toast.makeText(this, "NFC n'est pas disponible sur cet appareil.", Toast.LENGTH_LONG).show()
            return
        }

        if (nfcAdapter?.isEnabled == false) {
            Toast.makeText(this, "Veuillez activer NFC dans les paramètres.", Toast.LENGTH_LONG).show()
        }
    }

    override fun onResume() {
        super.onResume()
        enableForegroundDispatchSystem()
    }

    override fun onPause() {
        super.onPause()
        disableForegroundDispatchSystem()
    }

    override fun onNewIntent(intent: Intent) {
        super.onNewIntent(intent)

        try {
            if (NfcAdapter.ACTION_TAG_DISCOVERED == intent.action) {
                val tag: Tag? = if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.TIRAMISU) {
                    intent.getParcelableExtra(NfcAdapter.EXTRA_TAG, Tag::class.java)
                } else {
                    @Suppress("DEPRECATION")
                    intent.getParcelableExtra(NfcAdapter.EXTRA_TAG)
                }

                tag?.let {
                    val tagId = it.id
                    val tagInfo = "Tag détecté : ${bytesToHex(tagId)}"
                    tagInfoTextView.text = tagInfo
                }
            }
        } catch (e: Exception) {
            Toast.makeText(this, "Erreur lors de la lecture du tag NFC", Toast.LENGTH_LONG).show()
            e.printStackTrace() // Ajoutez un log pour comprendre la cause
        }
    }


    private fun enableForegroundDispatchSystem() {
        val intent = Intent(this, MainActivity::class.java).addFlags(Intent.FLAG_RECEIVER_REPLACE_PENDING)
        val pendingIntent = PendingIntent.getActivity(this, 0, intent, PendingIntent.FLAG_MUTABLE)

        val intentFilters = arrayOf(IntentFilter(NfcAdapter.ACTION_TAG_DISCOVERED))
        nfcAdapter?.enableForegroundDispatch(this, pendingIntent, intentFilters, null)
    }

    private fun disableForegroundDispatchSystem() {
        nfcAdapter?.disableForegroundDispatch(this)
    }

    private fun bytesToHex(bytes: ByteArray): String {
        return bytes.joinToString("") { String.format("%02X", it) }
    }
}